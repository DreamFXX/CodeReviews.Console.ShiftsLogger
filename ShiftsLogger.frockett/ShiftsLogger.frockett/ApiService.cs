﻿using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShiftsLogger.frockett.UI.Dtos;
using Spectre.Console;

namespace ShiftsLogger.frockett.UI;

public class ApiService
{
    private readonly HttpClient httpClient;
    private readonly string baseUri;

    public ApiService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    internal async Task<List<ShiftDto>> GetAllShifts()
    {
        string requestUrl = "shifts";

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        try
        {
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var shifts = JsonConvert.DeserializeObject<List<ShiftDto>>(content);

                return shifts;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    internal async Task AddShift(ShiftCreateDto newShift)
    {
        string requestUrl = "shifts";

        string newShiftJson = JsonConvert.SerializeObject(newShift);
        HttpContent content = new StringContent(newShiftJson, Encoding.UTF8, "application/json");

        try
        {
            var response = await httpClient.PostAsync(requestUrl, content);
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("New shift recorded. Press Enter to continue...");
            }
            else
            {
                Console.WriteLine($"Failed with status code {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was an error: {ex.Message}");
        }
    }

    internal async Task DeleteShift(int id)
    {
        string requestUrl = $"shifts/{id}";

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);
        try
        {
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Record deleted. Press Enter to continue...");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    internal async Task UpdateShift(ShiftDto updateShift)
    {
        string requestUrl = $"shifts/{updateShift.Id}";

        string newShiftJson = JsonConvert.SerializeObject(updateShift);
        HttpContent content = new StringContent(newShiftJson, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await httpClient.PutAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Update Successful. Press Enter to continue...");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    internal async Task<List<ShiftDto>>? GetShiftsByEmployeeId(int employeeId)
    {
        string requestUrl = $"employee/{employeeId}";

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

        try
        {
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var shifts = JsonConvert.DeserializeObject<List<ShiftDto>>(content);

                return shifts;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return null;
            }
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    internal async Task<ShiftDto>? GetShiftById(int id)
    {
        string requestUrl = $"shifts/{id}";

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

        try
        {
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var shift = JsonConvert.DeserializeObject<ShiftDto>(content);

                return shift;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    internal async Task<List<EmployeeDto>>? GetListOfEmployees()
    {
        string requestUrl = "employee";

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, requestUrl);

        try
        {
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var employees = JsonConvert.DeserializeObject<List<EmployeeDto>>(content);
                return employees;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            return null;
        }
    }

    internal async Task AddEmployee(EmployeeCreateDto newEmployee)
    {
        string requestUrl = "employee";

        //string newEmployeeJson = JsonConvert.SerializeObject(newEmployee);
        //HttpContent content = new StringContent(newEmployeeJson, Encoding.UTF8, "application/json");

        JsonContent content = JsonContent.Create(newEmployee);

        try
        {
            var response = await httpClient.PostAsync(requestUrl, content);
            if (response.IsSuccessStatusCode)
            {
                AnsiConsole.WriteLine($"{newEmployee.Name} has been added!");
            }
            else
            {
                Console.WriteLine($"Failed with status code {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was an error: {ex.Message}");
        }
    }
    internal async Task UpdateEmployee(EmployeeDto updatedEmployee)
    {
        string requestUrl = $"employee/{updatedEmployee.Id}";

        string newShiftJson = JsonConvert.SerializeObject(updatedEmployee);
        HttpContent content = new StringContent(newShiftJson, Encoding.UTF8, "application/json");

        try
        {
            HttpResponseMessage response = await httpClient.PutAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Update Successful. Press Enter to continue...");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    internal async Task DeleteEmployee(int id)
    {
        string requestUrl = $"employee/{id}";

        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, requestUrl);
        try
        {
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Record deleted. Press Enter to continue...");
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
