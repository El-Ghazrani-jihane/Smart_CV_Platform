namespace API.dtos.Matching;

public class SkillMatchDto
{
    public List<string> MatchingSkills { get; set; } = [];

    public List<string> MissingSkills { get; set; } = [];

    public double MatchPercentage { get; set; }
}