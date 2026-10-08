from fastapi import APIRouter

from app.models.skill_request import SkillRequest
from app.services.groq_skill_service import extract_skills

router = APIRouter(
    prefix="/skills",
    tags=["Skills"]
)


@router.post("/extract")
async def extract(job: SkillRequest):

    return extract_skills(job.text)