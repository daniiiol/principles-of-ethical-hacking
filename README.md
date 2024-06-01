# PRINCIPLES OF ETHICAL HACKING

This project presents a comprehensive webpage outlining the key principles that both hackers and organizations should adhere to in the realm of ethical hacking. These principles ensure that hacking activities are conducted responsibly, transparently, and legally, promoting trust and collaboration between all parties involved.

These principles are published on 🔗 <https://principles-of-ethical-hacking.org>.

# Translation

You will find all the language files in this 📂 [folder](.//src/PrinciplesOfEthicalHacking/website/data/i18n). If you would like to add or improve a language translation, please create it and send us a pull request.

# Discussions

We are excited to start a collaborative discussion on defining the fundamental principles of Ethical Hacking for both organizations that engage in ethical hacking and the hackers who practice it. Please use the discussion board of this repository for any questions, discussions or other feedback.

# Technical details

You don't need any technical knowledge to take part in this project. However, if you are interested in the technology behind it, you are welcome to take a look around.

## Local Run

In general, the environment is created via Docker. In the first stage, the website would be created by generating static files. In the second stage, these files are placed on a web server, which generates the final image.

### Build Docker

```bash
docker build . -t principles-of-ethical-hacking:dev
```

### Run Docker

```bash
docker run -p 80:80 principles-of-ethical-hacking:dev
```
