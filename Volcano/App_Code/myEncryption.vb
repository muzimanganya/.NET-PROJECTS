Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Security.Cryptography

Public Class myEncryption
    Public Shared Function Encrypt(strToEncrypt As String, strKey As String) As String
        Try
            Dim objDESCrypto As New TripleDESCryptoServiceProvider()
            Dim objHashMD5 As New MD5CryptoServiceProvider()
            Dim byteHash As Byte(), byteBuff As Byte()
            Dim strTempKey As String = strKey
            byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey))
            objHashMD5 = Nothing
            objDESCrypto.Key = byteHash
            objDESCrypto.Mode = CipherMode.ECB
            'CBC, CFB
            byteBuff = ASCIIEncoding.ASCII.GetBytes(strToEncrypt)
            Return Convert.ToBase64String(objDESCrypto.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length))
        Catch ex As Exception
            Return "Wrong Input. " + ex.Message
        End Try
    End Function
    Public Shared Function Decrypt(strEncrypted As String, strKey As String) As String
        Try
            Dim objDESCrypto As New TripleDESCryptoServiceProvider()
            Dim objHashMD5 As New MD5CryptoServiceProvider()
            Dim byteHash As Byte(), byteBuff As Byte()
            Dim strTempKey As String = strKey
            byteHash = objHashMD5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(strTempKey))
            objHashMD5 = Nothing
            objDESCrypto.Key = byteHash
            objDESCrypto.Mode = CipherMode.ECB
            'CBC, CFB
            byteBuff = Convert.FromBase64String(strEncrypted)
            Dim strDecrypted As String = ASCIIEncoding.ASCII.GetString(objDESCrypto.CreateDecryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length))
            objDESCrypto = Nothing
            Return strDecrypted
        Catch ex As Exception
            Return "Wrong Input. " + ex.Message
        End Try
    End Function

End Class
