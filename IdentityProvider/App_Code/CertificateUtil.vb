Imports System.Security.Cryptography.X509Certificates


Public Class CertificateUtil
    Public Shared Function GetCertificate(name As StoreName, location As StoreLocation, subjectName As String) As X509Certificate2
        Dim store As New X509Store(name, location)
        Dim certificates As X509Certificate2Collection = Nothing
        store.Open(OpenFlags.[ReadOnly])

        Try
            Dim result As X509Certificate2 = Nothing

            '
            ' Every time we call store.Certificates property, a new collection will be returned.
            '
            certificates = store.Certificates

            For i As Integer = 0 To certificates.Count - 1
                Dim cert As X509Certificate2 = certificates(i)

                If cert.SubjectName.Name.ToLower() = subjectName.ToLower() Then
                    If result IsNot Nothing Then
                        Throw New ApplicationException(String.Format("There are multiple certificates for subject Name {0}", subjectName))
                    End If

                    result = New X509Certificate2(cert)
                End If
            Next

            If result Is Nothing Then
                Throw New ApplicationException(String.Format("No certificate was found for subject Name {0}", subjectName))
            End If

            Return result
        Finally
            If certificates IsNot Nothing Then
                For i As Integer = 0 To certificates.Count - 1
                    Dim cert As X509Certificate2 = certificates(i)
                    cert.Reset()
                Next
            End If

            store.Close()
        End Try
    End Function
End Class

