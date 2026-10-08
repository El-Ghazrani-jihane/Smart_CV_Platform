import os
import json

from groq import Groq
from dotenv import load_dotenv

from app.prompts.skill_prompt import build_skill_prompt

load_dotenv()

client = Groq(
    api_key=os.getenv("GROQ_API_KEY")
)


def extract_skills(text: str):

    prompt = build_skill_prompt(text)

    response = client.chat.completions.create(
        model="llama-3.3-70b-versatile",
        messages=[
            {
                "role": "user",
                "content": prompt
            }
        ],
        temperature=0
    )

    content = response.choices[0].message.content

    content = content.replace("```json", "")
    content = content.replace("```", "")
    content = content.strip()

    return json.loads(content)