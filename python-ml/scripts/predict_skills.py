import spacy

def extract_skills(description):
    nlp = spacy.load("models/skill_extractor_model")
    doc = nlp(description)

    # Extract entities labeled as SKILL
    skills = [ent.text for ent in doc.ents if ent.label_ == "SKILL"]
    return skills

if __name__ == "__main__":
    description = "We are looking for a nurse with Python and SQL skills."
    print(extract_skills(description))
