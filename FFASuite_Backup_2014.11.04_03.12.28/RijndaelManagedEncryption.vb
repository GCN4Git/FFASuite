Imports System.ComponentModel
Imports System.Security.Cryptography
Imports System.Text
Imports System.IO


#Region " Rijndael Encryption Algorithm "

Public Class RijndaelManagedEncryption

#Region " Global Variables "
    Private Shared encoding As New UTF8Encoding
    Private Shared RijndaelAlg As New RijndaelManaged
    Private Shared HashAlg As HashAlgorithm = New SHA256Managed
    Public Shared CryptoKey As String = "D33F02FF3A3476E48FA380C2DE34B087C67D74B6C524B0CD03C615B0DAA5C31B0F8DFB2E2800AA14AB4874ECF0F1"

    Enum RijndaelCryptographicAction
        Encrypt = 0
        Decrypt = 1
    End Enum
#End Region

#Region " Encryption/Decryption Events "
    Public Overloads Shared Function EncryptDecrypt(ByVal text As String, ByVal key As String, ByVal action As RijndaelCryptographicAction) As String
        Dim textBytes As Byte() = encoding.GetBytes(text)
        Dim keyBytes As Byte() = CheckKey(key)

        Return EncryptDecrypt(textBytes, keyBytes, action)
    End Function
    Public Overloads Shared Function EncryptDecrypt(ByVal text As String, ByVal key As Byte(), _
                                                    ByVal action As RijndaelCryptographicAction) As String
        Dim textBytes As Byte() = encoding.GetBytes(text)
        Dim keyBytes As Byte() = CheckKey(key)

        Return EncryptDecrypt(textBytes, keyBytes, action)
    End Function
    Public Overloads Shared Function EncryptDecrypt(ByVal data As Byte(), ByVal key As String, _
                                                    ByVal action As RijndaelCryptographicAction) As String

        Dim keyBytes As Byte() = CheckKey(key)

        Return EncryptDecrypt(data, keyBytes, action)
    End Function
    Public Overloads Shared Function EncryptDecrypt(ByVal data As Byte(), ByVal key As Byte(), _
                                                    ByVal action As RijndaelCryptographicAction) As String

        RijndaelAlg.BlockSize = 256
        If Not key.Length = 32 Then
            key = CheckKey(key)
        End If
        Dim IV As Byte() = GenerateIV(key)
        Try
            Select Case action
                Case RijndaelCryptographicAction.Encrypt
                    Using memStream As New MemoryStream
                        Using cStream As New CryptoStream(memStream, _
                                                   RijndaelAlg.CreateEncryptor(key, IV), _
                                                   CryptoStreamMode.Write)
                            cStream.Write(data, 0, data.Length)
                        End Using
                        Return Convert.ToBase64String(memStream.ToArray)
                    End Using
                Case RijndaelCryptographicAction.Decrypt
                    Dim textBytes As Byte() = Convert.FromBase64String(encoding.GetString(data))
                    Using memStream As New MemoryStream(textBytes)
                        Dim decryptedData(textBytes.Length) As Byte
                        Using cStream As New CryptoStream(memStream, _
                                                   RijndaelAlg.CreateDecryptor(key, IV), _
                                                   CryptoStreamMode.Read)
                            cStream.Read(decryptedData, 0, decryptedData.Length)
                        End Using
                        Return encoding.GetString(decryptedData)
                    End Using
                Case Else
                    Return Nothing
            End Select
        Catch ex As CryptographicException
            Return "Error - Invalid Password"
        End Try
    End Function
#End Region

    'Generate a byte array containing the generated hash from the given key.
#Region " Key Check "
    Private Overloads Shared Function CheckKey(ByVal key As String) As Byte()
        Dim byteKey As Byte() = encoding.GetBytes(key)

        Return CheckKey(byteKey)
    End Function

    Private Overloads Shared Function CheckKey(ByVal key As Byte()) As Byte()

        Return HashAlg.ComputeHash(key)

    End Function

#End Region


    'Generate a byte array containing the generated IV from the given key.

#Region " Generate IV "

    Private Overloads Shared Function GenerateIV(ByVal key As String) As Byte()

        Dim keyBytes As Byte() = encoding.GetBytes(key)

        Return GenerateIV(keyBytes)
    End Function


    Private Overloads Shared Function GenerateIV(ByVal key As Byte()) As Byte()

        Dim keyReverse As String = encoding.GetString(key).ToCharArray.Reverse.ToString

        Dim NewKey As Byte() = encoding.GetBytes(keyReverse)

        Return HashAlg.ComputeHash(NewKey)

    End Function

#End Region

End Class
#End Region