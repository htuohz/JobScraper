import spacy
import re

# Initialize spaCy model
nlp = spacy.load("en_core_web_sm")

def extract_skills(description: str) -> set:
    """
    Extract potential skills from job descriptions using NER and pattern matching.

    Args:
        description (str): The job description text.

    Returns:
        set: Extracted skills.
    """
    doc = nlp(description)
    
    # Define patterns for technical skills
    technical_terms = set()
    
    # NER extraction
    for ent in doc.ents:
        if ent.label_ in {"ORG", "PRODUCT", "WORK_OF_ART"}:  # Expand labels as needed
            technical_terms.add(ent.text.strip())
    
    # Regex-based pattern matching for technical terms
    patterns = [
        r"\b(Java|C#|SQL|Lucene|Oracle|Spring/Boot|Hibernate|Tomcat|Docker|Git)\b",
        r"\b(JavaScript|Python|AWS|Kubernetes|Azure)\b",
        r"\bdata (analysis|engineering|management)\b",
    ]
    
    for pattern in patterns:
        matches = re.findall(pattern, description, flags=re.IGNORECASE)
        technical_terms.update([match.strip() for match in matches])

    # Deduplicate and return extracted terms
    return {term for term in technical_terms if len(term) > 1}

# Test the extraction
if __name__ == "__main__":
    job_description = """
   About the Opportunity:

    Are you an experienced Full Stack Developer seeking a permanent role with growth opportunities? We are looking for a talented individual with expertise in Angular or React and C# backend development to join a dynamic team. This full-time role offers a competitive salary of $150,000–$165,000 + Super and the chance to work across a variety of impactful projects.

    About You:

    We’re seeking a proactive Full Stack Developer with a balance of technical expertise and a growth-oriented mindset. You’ll thrive in a collaborative environment and bring strong problem-solving skills to a range of architectural, development, and support tasks.

    You should have:


        A minimum of 4–5 years of experience with Angular or React (Angular preferred).
        Strong backend development skills in C# and full stack capabilities.
        Ideally, around 7 years of experience in software development overall.
        Proven ability to deliver end-to-end systems and solution data transformations.
        Strong teamwork and communication skills to work effectively within a team of 15 professionals.


    Key Responsibilities:


        Develop and deliver systems across a broad range of projects, ensuring quality and resilience through unit testing and best practices.
        Work on architectural and strategic initiatives, including solution design and data transformation.
        Handle a variety of tasks, from project work to support.
        Act as an individual contributor, collaborating closely with team members and reporting to the manager.


    The Benefits:


        Competitive salary of $150,000–$165,000 + Super.
        Access to a structured development plan with certifications and training to support professional growth.
        Opportunities to work across a diverse range of projects and avoid being confined to a single function.
        A supportive environment that invests in employee development and fosters a growth mindset.
        Hybrid role (3 days in the office)&nbsp;


    How to Apply:

    If this opportunity sounds like a great fit for you, please apply now. For further details or enquiries, feel free to reach out directly via email at [email&#160;protected], or call Matthew Burke on (08) 6146 4466.
    """  # Paste your job description here
    extracted_skills = extract_skills(job_description)
    print("Extracted Skills:", extracted_skills)
