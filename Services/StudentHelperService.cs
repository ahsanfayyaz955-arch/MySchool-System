using System.Net.Http.Json;
using MySchool_System.Models;

public class StudentApiService
{
    private readonly HttpClient _http;

    public StudentApiService(HttpClient http, IConfiguration config)
    {
        _http = http;
        _http.BaseAddress = new Uri(config["ApiSettings:BaseUrl"]);
    }

    public async Task<List<Student>> GetStudentsAsync()
    {
        try
        {
            return await _http.GetFromJsonAsync<List<Student>>("api/students")
                   ?? new List<Student>();
        }
        catch
        {
            return new List<Student>();
        }
    }

    public async Task<Student?> GetStudentAsync(int id)
    {
        return await _http.GetFromJsonAsync<Student>($"api/students/{id}");
    }

    public async Task<bool> AddStudentAsync(Student student)
    {
        var response = await _http.PostAsJsonAsync("api/students", student);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateStudentAsync(Student student)
    {
        var response = await _http.PutAsJsonAsync($"api/students/{student.Id}", student);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteStudentAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/students/{id}");
        return response.IsSuccessStatusCode;
    }
}
