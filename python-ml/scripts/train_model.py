import spacy
from spacy.training import Example

TRAIN_DATA = [
    ("We need a nurse skilled in Python.", {"entities": [(22, 28, "SKILL")]}),
    ("Experience with SQL and Docker.", {"entities": [(13, 16, "SKILL"), (21, 27, "SKILL")]}),
]

# Load blank model
nlp = spacy.blank("en")
ner = nlp.add_pipe("ner")
ner.add_label("SKILL")

# Prepare training data
examples = [Example.from_dict(nlp.make_doc(text), annotations) for text, annotations in TRAIN_DATA]

# Train model
optimizer = nlp.initialize()
for epoch in range(10):
    for example in examples:
        nlp.update([example], sgd=optimizer)

# Save the trained model
nlp.to_disk("models/skill_extractor_model")
print("Model training complete!")
