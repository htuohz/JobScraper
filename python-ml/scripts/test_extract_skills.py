from extract_skills import extract_skills_with_ner

if __name__ == "__main__":
    # Example job description
    job_description = """
Hiring Reporters NowGlobal private equity-backed news publication Azzet is seeking Reporters 
to join our fast-growing Australian team.Our vibrant finance editorial team covers the most important business and personal finance news of the day, and we are looking for someone with the right blend of business nous and burgeoning editorial flair to join us on our exciting journey.The roleThis is an excellent, remote, entry-level role, perfect for candidates who are keen to learn more about Australia’s business community and work as part of a greater editorial team. After 12 months in this role, you will have developed a greater understanding of the Australian financial landscape, built up a wide portfolio of business-focused articles, and have the opportunity to manage additional digital content to drive results for our business.RequirementsA completed Bachelor's degree in Journalism or CommunicationsA keen understanding and/or interest in Australian and global business newsExcellent written and verbal skills with great attention to detail The ability to write clean, fast copy in the morning for a newsletter deadlineExperience in a newsroom environment is greatly preferredExperience in writing finance stories is a tremendous advantageThe ability to write longer form pieces around a topic or an interview subjectThe ability to take and make phone calls as well as write emailsAn understanding of the Australian digital media industry and landscapeThe ability to work in a remote environment and stay connectedSomeone who can learn quickly come up with fresh story ideas and anglesA “can do” attitude - someone who is curious and goes the extra mile for a storyA creative and innovative mind - thinking outside the box on storytelling The desire and ability to thrive in a dynamic, fast-paced team, and work to deadline, working with content in a timely manner each dayRemote availableThis role is remote and is available to Reporters across Australia.
    """
    # Call the skill extraction function
    skills = extract_skills_with_ner(job_description)

    # Print the extracted skills
    print("Extracted Skills:", skills)
