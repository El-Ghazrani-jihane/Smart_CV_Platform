def build_skill_prompt(text: str) -> str:
    return f"""
Extract all technical skills from the following job offer.

Rules:
- Return ONLY valid JSON.
- No markdown.
- No explanations.
- No comments.

Format:

{{
    "skills": [
        "Skill1",
        "Skill2"
    ]
}}

Job Offer:

{text}
"""