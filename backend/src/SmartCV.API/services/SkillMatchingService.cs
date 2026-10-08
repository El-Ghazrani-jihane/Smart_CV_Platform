using System.Text;
using System.Text.Json;
using API.data;
using API.dtos.Matching;
using Microsoft.EntityFrameworkCore;

namespace API.services;

public class SkillMatchingService
{
    private readonly ApplicationDbContext _context;
    private readonly HttpClient _httpClient;

    public SkillMatchingService(
        ApplicationDbContext context,
        HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task<SkillMatchDto> CompareAsync(
        int profilId,
        string jobDescription)
    {
        Console.WriteLine($"Profil demandé : {profilId}");

        var profil = await _context.Profils
            .Include(p => p.Competences)
            .FirstOrDefaultAsync(p => p.Id == profilId);

        if (profil == null)
        {
            var existingIds = await _context.Profils
                .Select(p => p.Id)
                .ToListAsync();

            throw new Exception(
                $"Profil introuvable. IDs existants : {string.Join(", ", existingIds)}");
        }

        var requestBody = new
        {
            text = jobDescription
        };

        var content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(
            "http://localhost:8000/skills/extract",
            content);

        response.EnsureSuccessStatusCode();

        var jsonResponse =
            await response.Content.ReadAsStringAsync();

        var extractedSkills =
            JsonSerializer.Deserialize<SkillsResponse>(
                jsonResponse,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

        if (extractedSkills == null)
        {
            throw new Exception(
                "Impossible de récupérer les compétences extraites.");
        }

        var profileSkills = profil.Competences
            .Select(c => c.Nom.Trim())
            .ToList();

        var offerSkills = extractedSkills.Skills
            .Select(s => s.Trim())
            .ToList();

        var matchingSkills = offerSkills
            .Where(skill =>
                profileSkills.Any(profileSkill =>
                    profileSkill.Equals(
                        skill,
                        StringComparison.OrdinalIgnoreCase)))
            .Distinct()
            .ToList();

        var missingSkills = offerSkills
            .Where(skill =>
                !profileSkills.Any(profileSkill =>
                    profileSkill.Equals(
                        skill,
                        StringComparison.OrdinalIgnoreCase)))
            .Distinct()
            .ToList();

        double percentage = 0;

        if (offerSkills.Count > 0)
        {
            percentage =
                (double)matchingSkills.Count
                / offerSkills.Count
                * 100;
        }

        return new SkillMatchDto
        {
            MatchingSkills = matchingSkills,
            MissingSkills = missingSkills,
            MatchPercentage = Math.Round(percentage, 2)
        };
    }

    private class SkillsResponse
    {
        public List<string> Skills { get; set; } = new();
    }
}