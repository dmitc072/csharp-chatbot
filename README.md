# C# Chatbot

A clean, minimal .NET 8 console chatbot with pluggable LLM backends (OpenAI-compatible by default). Built for clarity, testing, and CI from day one.

![build](https://img.shields.io/github/actions/workflow/status/dmitc072/csharp-chatbot/dotnet.yml?branch=main)
![codeql](https://img.shields.io/github/actions/workflow/status/dmitc072/csharp-chatbot/codeql.yml?label=codeql)
![license](https://img.shields.io/badge/license-MIT-blue.svg)

## Features

- 📦 .NET 8, idiomatic Program/Main via Generic Host + DI
- 🔌 Pluggable `IChatClient` (OpenAI-compatible example included)
- 🧪 xUnit tests with 80%+ coverage target
- 🔄 GitHub Actions: restore → build → test → coverage artifact
- 🛡️ CodeQL + Dependabot

## Quick start

```bash
# 1) requirements
- .NET SDK 8.0+
- (optional) OPENAI_API_KEY in env if using OpenAI-compatible client

# 2) clone & run
git clone https://github.com/dmitc072/csharp-chatbot
cd csharp-chatbot

dotnet run
```
