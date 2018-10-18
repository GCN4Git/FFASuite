Imports System.IO.Compression
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Class XMPPZipClass
    Public Function Compress(text As String) As String
        Dim buffer As Byte() = Encoding.UTF8.GetBytes(text)
        Dim ms As New MemoryStream()
        Using zip As New GZipStream(ms, CompressionMode.Compress, True)
            zip.Write(buffer, 0, buffer.Length)
        End Using
        ms.Position = 0
        Dim outStream As New MemoryStream()
        Dim compressed As Byte() = New Byte(CInt(ms.Length) - 1) {}
        ms.Read(compressed, 0, compressed.Length)
        Dim gzBuffer As Byte() = New Byte(compressed.Length + 3) {}
        System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length)
        System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4)
        Return Convert.ToBase64String(gzBuffer)
    End Function

    Public Function Decompress(compressedText As String) As String
        Dim gzBuffer As Byte() = Convert.FromBase64String(compressedText)
        Using ms As New MemoryStream()
            Dim msgLength As Integer = BitConverter.ToInt32(gzBuffer, 0)
            ms.Write(gzBuffer, 4, gzBuffer.Length - 4)
            Dim buffer As Byte() = New Byte(msgLength - 1) {}
            ms.Position = 0
            Using zip As New GZipStream(ms, CompressionMode.Decompress)
                zip.Read(buffer, 0, buffer.Length)
            End Using
            Return Encoding.UTF8.GetString(buffer)
        End Using
    End Function

    Public Function ToXml(obj As Object) As String
        ' remove the default namespaces
        Dim ns As New XmlSerializerNamespaces()
        ns.Add(String.Empty, String.Empty)
        ' serialize to string
        Dim xs As New XmlSerializer(obj.[GetType]())
        Dim sw As New StringWriter()
        xs.Serialize(sw, obj, ns)
        Return sw.GetStringBuilder().ToString()
    End Function
    Public Function FromXml(Xml As String, ObjType As System.Type) As Object

        Dim ser As XmlSerializer
        ser = New XmlSerializer(ObjType)
        Dim stringReader As StringReader
        stringReader = New StringReader(Xml)
        Dim xmlReader As XmlTextReader
        xmlReader = New XmlTextReader(stringReader)
        Dim obj As Object
        obj = ser.Deserialize(xmlReader)
        xmlReader.Close()
        stringReader.Close()
        Return obj

    End Function
    Public Function FromXml(Xml As String, obj As Object) As Object

        Dim ser As XmlSerializer
        ser = New XmlSerializer(obj.[GetType]())
        Dim stringReader As StringReader
        stringReader = New StringReader(Xml)
        Dim xmlReader As XmlTextReader
        xmlReader = New XmlTextReader(stringReader)
        obj = ser.Deserialize(xmlReader)
        xmlReader.Close()
        stringReader.Close()
        Return obj
    End Function
End Class
