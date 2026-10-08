from pydantic import BaseModel

class SkillRequest(BaseModel):
    text: str