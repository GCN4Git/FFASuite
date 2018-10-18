Imports System.Net
Imports System.Text
Imports System.IO


Public Class BalticFTPClass

    Public FullDirectoryFiles As New List(Of SingleFileFTPClass)
    Public FullDirectoryListing As New List(Of String)

    Public FTPReponse As New FTPResponseClass

    Private ftpServerIP As String
    Private ftpUserID As String
    Private ftpPassword As String

    Public Sub New(ByVal _ftpServerIP As String, ByVal _ftpUserID As String, ByVal _ftpPassword As String)
        ftpServerIP = _ftpServerIP
        ftpUserID = _ftpUserID
        ftpPassword = _ftpPassword
    End Sub

    Public Sub FTPSelectedFiles(ByRef _FileList As List(Of SingleFileFTPClass))

        Dim URI As String
        Dim ServerURI As Uri
        Dim reqFTP As System.Net.FtpWebRequest = Nothing

        Dim response As FtpWebResponse = Nothing
        Dim responseStream As Stream = Nothing
        Dim reader As StreamReader = Nothing

        For Each remoteFile In _FileList
            Try
                URI = remoteFile.FilePath
                ServerURI = New Uri(URI)
                reqFTP = CType(FtpWebRequest.Create(URI), FtpWebRequest)
                reqFTP.Credentials = New NetworkCredential(ftpUserID, ftpPassword)
                reqFTP.UseBinary = True
                reqFTP.Proxy = Nothing
                reqFTP.KeepAlive = False
                reqFTP.UsePassive = False
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile

                response = reqFTP.GetResponse
                responseStream = response.GetResponseStream
                reader = New StreamReader(responseStream)

                remoteFile.FileStr = reader.ReadToEnd
                remoteFile.FileError = FTPResponses.FTPFileReadOK
                remoteFile.ErrorMsg = "OK"

                reader.Close()
                responseStream.Close()
                response.Close()
                reqFTP = Nothing
            Catch ex As Exception
                If IsNothing(response) = False Then response.Close()
                If IsNothing(reader) = False Then reader.Close()
                If IsNothing(reqFTP) = False Then reqFTP.Abort() : reqFTP = Nothing

                remoteFile.FileError = FTPResponses.FTPFileReadingError
                remoteFile.ErrorMsg = "Error while reading file: " & remoteFile.FileName & " Error: " & ex.Message.ToString
                FTPReponse.FTPError = FTPResponses.FTPFileReadingError
                FTPReponse.ErrorMsg = "Error while reading file: " & remoteFile.FileName & " Error: " & ex.Message.ToString
                Exit Sub
            End Try
        Next
        FTPReponse.FTPError = FTPResponses.FTPFileReadOK
        FTPReponse.ErrorMsg = "OK"
    End Sub

    Public Sub GetAllFilesInDirectory()
        Dim downloadFiles As String = ""
        Dim result As New StringBuilder
        Dim response As WebResponse = Nothing
        Dim reader As StreamReader = Nothing
        Dim line As String = ""

        Dim reqFTP As System.Net.FtpWebRequest = CType(FtpWebRequest.Create(New Uri("ftp://" & ftpServerIP + "/")), FtpWebRequest)
        reqFTP.Credentials = New NetworkCredential(ftpUserID, ftpPassword)
        reqFTP.UseBinary = True
        reqFTP.Method = WebRequestMethods.Ftp.ListDirectory
        reqFTP.Proxy = Nothing
        reqFTP.KeepAlive = False
        reqFTP.UsePassive = False

        Try
            response = reqFTP.GetResponse()
            reader = New StreamReader(response.GetResponseStream())
            line = reader.ReadLine()

            While line <> ""
                result.Append(line)
                result.Append(";")
                line = reader.ReadLine()
            End While
            'to remove the trailing ';'
            result.Remove(result.ToString().LastIndexOf(";"), 1)
            reader.Close()
            response.Close()

            For Each row In result.ToString().Split(";").ToList
                FullDirectoryListing.Add(row)
                Dim nf As New SingleFileFTPClass
                nf.FilePath = "ftp://" & ftpServerIP + "/" & row
                nf.FileName = row
                If row.Contains("CMSIA") Then
                    nf.FileType = BalticFileType.CMSIA
                    nf.BalticDate = DateSerial(row.Substring(6, 4), row.Substring(10, 2), row.Substring(12, 2))
                ElseIf row.Contains("CMSRA") Then
                    nf.FileType = BalticFileType.CMSRA
                    nf.BalticDate = DateSerial(row.Substring(6, 4), row.Substring(10, 2), row.Substring(12, 2))
                ElseIf row.Contains("BSD_ALL") Then
                    nf.FileType = BalticFileType.BSDALL
                    nf.BalticDate = DateSerial(row.Substring(7, 4), row.Substring(11, 2), row.Substring(13, 2))
                ElseIf row.Contains("BSD") Then
                    nf.FileType = BalticFileType.BSD
                    nf.BalticDate = DateSerial(row.Substring(4, 4), row.Substring(8, 2), row.Substring(10, 2))
                End If
                FullDirectoryFiles.Add(nf)
            Next
            FTPReponse.FTPError = FTPResponses.FTPDirectoryDownloaded
        Catch ex As Exception
            MessageBox.Show("FTP Request Failed with message: " & ex.Message)
            If IsNothing(response) = False Then response.Close()
            If IsNothing(reader) = False Then reader.Close()
            If IsNothing(reqFTP) = False Then reqFTP.Abort() : reqFTP = Nothing

            FTPReponse.FTPError = FTPResponses.ErrorReadingFTPDirectory
            FTPReponse.ErrorMsg = "Error Reading FTP Direcory"
        End Try

        reqFTP = CType(FtpWebRequest.Create(New Uri("ftp://" & ftpServerIP + "/archive/")), FtpWebRequest)
        reqFTP.Credentials = New NetworkCredential(ftpUserID, ftpPassword)
        reqFTP.UseBinary = True
        reqFTP.Method = WebRequestMethods.Ftp.ListDirectory
        reqFTP.Proxy = Nothing
        reqFTP.KeepAlive = False
        reqFTP.UsePassive = False

        Try
            response = reqFTP.GetResponse()
            reader = New StreamReader(response.GetResponseStream())
            line = reader.ReadLine()
            result = New StringBuilder

            While line <> ""
                result.Append(line)
                result.Append(";")
                line = reader.ReadLine()
            End While
            'to remove the trailing ';'
            result.Remove(result.ToString().LastIndexOf(";"), 1)
            reader.Close()
            response.Close()

            For Each row In result.ToString().Split(";").ToList
                FullDirectoryListing.Add(row)
                Dim nf As New SingleFileFTPClass
                nf.FilePath = "ftp://" & ftpServerIP + "/archive/" & row
                nf.FileName = row
                If row.Contains("CMSIA") Then
                    nf.FileType = BalticFileType.CMSIA
                    nf.BalticDate = DateSerial(row.Substring(6, 4), row.Substring(10, 2), row.Substring(12, 2))
                ElseIf row.Contains("CMSRA") Then
                    nf.FileType = BalticFileType.CMSRA
                    nf.BalticDate = DateSerial(row.Substring(6, 4), row.Substring(10, 2), row.Substring(12, 2))
                ElseIf row.Contains("BSD_ALL") Then
                    nf.FileType = BalticFileType.BSDALL
                    nf.BalticDate = DateSerial(row.Substring(7, 4), row.Substring(11, 2), row.Substring(13, 2))
                ElseIf row.Contains("BSD") Then
                    nf.FileType = BalticFileType.BSD
                    nf.BalticDate = DateSerial(row.Substring(4, 4), row.Substring(8, 2), row.Substring(10, 2))
                End If
                FullDirectoryFiles.Add(nf)
            Next
            FTPReponse.FTPError = FTPResponses.FTPDirectoryDownloaded
        Catch ex As Exception
            If IsNothing(response) = False Then response.Close()
            If IsNothing(reader) = False Then reader.Close()
            If IsNothing(reqFTP) = False Then reqFTP.Abort() : reqFTP = Nothing

            FTPReponse.FTPError = FTPResponses.ErrorReadingFTPDirectory
            FTPReponse.ErrorMsg = "Error Reading FTP Direcory"
        End Try

    End Sub

    Public Class SingleFileFTPClass
        Private m_FileName As String = String.Empty
        Private m_FilePath As String = String.Empty
        Private m_FileStr As String = String.Empty
        Private m_FileType As BalticFileType
        Private m_BalticDate As Date
        Private m_FileError As FTPResponses
        Private m_ErrorMsg As String = String.Empty

        Public Property FileName As String
            Get
                Return m_FileName
            End Get
            Set(value As String)
                m_FileName = value
            End Set
        End Property
        Public Property FilePath As String
            Get
                Return m_FilePath
            End Get
            Set(value As String)
                m_FilePath = value
            End Set
        End Property
        Public Property FileStr As String
            Get
                Return m_FileStr
            End Get
            Set(value As String)
                m_FileStr = value
            End Set
        End Property
        Public Property FileType As BalticFileType
            Get
                Return m_FileType
            End Get
            Set(value As BalticFileType)
                m_FileType = value
            End Set
        End Property
        Public Property BalticDate As Date
            Get
                Return m_BalticDate
            End Get
            Set(value As Date)
                m_BalticDate = value
            End Set
        End Property
        Public Property FileError As FTPResponses
            Get
                Return m_FileError
            End Get
            Set(value As FTPResponses)
                m_FileError = value
            End Set
        End Property
        Public Property ErrorMsg As String
            Get
                Return m_ErrorMsg
            End Get
            Set(value As String)
                m_ErrorMsg = value
            End Set
        End Property
    End Class
    Public Class FTPResponseClass
        Public FTPError As FTPResponses = FTPResponses.FTPDirectoryDownloaded
        Public ErrorMsg As String = ""
    End Class
    Enum BalticFileType
        CMSIA
        CMSRA
        BSD
        BSDALL
    End Enum
    Enum FTPResponses
        FTPDirectoryIsEmpty
        ErrorReadingFTPDirectory
        FTPDirectoryDownloaded
        NoFilesForSelectedDate
        FTPFileReadOK
        FTPFileReadingError
    End Enum

End Class

