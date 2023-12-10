Imports System.Net.Http
Imports Newtonsoft.Json

Module Module1
    Sub Main()
        Dim httpClient As New HttpClient()

        ' Kullanıcı adı ve şifre
        Dim username As String = "exampleUser"
        Dim password As String = "examplePassword"

        ' Şifreyi SHA-256 ile hashle
        Dim hashedPassword As String = GetSha256Hash(password)

        ' Login isteği için JSON oluştur
        Dim requestData As New With {
            .username = username,
            .password = hashedPassword
        }

        ' Login isteğini yap
        Dim responseContent As String = PostRequest(httpClient, "http://localhost:3000/login", requestData)

        ' Yanıtı değerlendir
        If responseContent = "Login Successful!" Then
            Console.WriteLine("Login Successful!")
        Else
            Console.WriteLine("Login Failed!")
        End If

        Console.ReadLine()
    End Sub

    ' SHA-256 hash üretme fonksiyonu
    Function GetSha256Hash(input As String) As String
        Using sha256 As New System.Security.Cryptography.SHA256Managed()
            Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(input)
            Dim hash As Byte() = sha256.ComputeHash(bytes)
            Return BitConverter.ToString(hash).Replace("-", "").ToLower()
        End Using
    End Function

    ' POST isteği yapma fonksiyonu
    Function PostRequest(client As HttpClient, url As String, data As Object) As String
        Dim json = JsonConvert.SerializeObject(data)
        Dim content = New StringContent(json, System.Text.Encoding.UTF8, "application/json")
        Dim response = client.PostAsync(url, content).Result
        Return response.Content.ReadAsStringAsync().Result
    End Function
End Module
