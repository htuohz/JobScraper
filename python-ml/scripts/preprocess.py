import spacy

def preprocess_text(description):
    nlp = spacy.load("en_core_web_sm")  # Load small spaCy model
    doc = nlp(description)

    # Tokenize and filter stop words
    tokens = [token.text.lower() for token in doc if not token.is_stop and token.is_alpha]
    return tokens

if __name__ == "__main__":
    sample_description = "We are looking for a nurse skilled in Python and SQL."
    print(preprocess_text(sample_description))
