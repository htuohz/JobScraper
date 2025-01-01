import requests

def send_skills_to_backend(job_id, skills):
    url = "http://localhost:5182/api/skills"
    payload = {
        "jobId": job_id,
        "skills": skills
    }

    response = requests.post(url, json=payload)
    if response.status_code == 200:
        print(f"Successfully sent skills for Job ID: {job_id}")
    else:
        print(f"Failed to send skills: {response.status_code}")

if __name__ == "__main__":
    job_id = 123
    skills = ["Python", "SQL", "Docker"]
    send_skills_to_backend(job_id, skills)
