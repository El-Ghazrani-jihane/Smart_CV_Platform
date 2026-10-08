from fastapi import FastAPI
from app.routes import analyze, cover_letter, competence
from fastapi.middleware.cors import CORSMiddleware
from app.routes.skill_routes import router as skill_router
app = FastAPI(title="Cevia AI Service")
app.add_middleware(
    CORSMiddleware,
    allow_origins=["http://localhost", "http://localhost:80"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

app.include_router(analyze.router)
app.include_router(cover_letter.router)
app.include_router(competence.router)
app.include_router(skill_router)
@app.get("/")
def root():
    return {"status": "AI Service running"}
