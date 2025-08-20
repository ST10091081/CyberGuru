// Sample Cybersecurity Quiz Questions
const quizData = [
    {
        question: "Which of the following is a strong password?",
        options: ["123456", "Password!", "MyDog2025!", "qwerty"],
        answer: 2
    },
    {
        question: "What should you do if you receive a suspicious email?",
        options: [
            "Click the link to check it",
            "Ignore it completely",
            "Report it to IT/security team",
            "Reply asking for more details"
        ],
        answer: 2
    },
    {
        question: "Phishing is an attempt to:",
        options: [
            "Speed up internet connection",
            "Steal sensitive information",
            "Clean malware from a computer",
            "Improve password strength"
        ],
        answer: 1
    }
];

let currentQuestion = 0;
let score = 0;
let timeLeft = 15;
let timer;

// Load Question
function loadQuestion() {
    document.getElementById("question-box").innerText = quizData[currentQuestion].question;
    let optionsBox = document.getElementById("options-box");
    optionsBox.innerHTML = "";

    quizData[currentQuestion].options.forEach((option, index) => {
        let btn = document.createElement("button");
        btn.innerText = option;
        btn.classList.add("btn", "btn-outline-primary", "m-1");
        btn.onclick = () => checkAnswer(index);
        optionsBox.appendChild(btn);
    });

    document.getElementById("next-btn").disabled = true;

    // Reset & Start Timer
    timeLeft = 15;
    document.getElementById("time").innerText = timeLeft;
    clearInterval(timer);
    timer = setInterval(updateTimer, 1000);
}

// Timer Countdown
function updateTimer() {
    timeLeft--;
    document.getElementById("time").innerText = timeLeft;
    if (timeLeft <= 0) {
        clearInterval(timer);
        alert("⏳ Time's up!");
        nextQuestion();
    }
}

// Check Answer
function checkAnswer(selected) {
    clearInterval(timer);
    let correctAnswer = quizData[currentQuestion].answer;
    if (selected === correctAnswer) {
        let bonus = timeLeft > 5 ? 5 : 0; // Bonus points if answered quickly
        score += 20 + bonus;
        alert("✅ Correct! +" + (20 + bonus) + " points");
    } else {
        score -= 5;
        alert("❌ Wrong! -5 points");
    }
    document.getElementById("score").innerText = "Score: " + score;
    document.getElementById("next-btn").disabled = false;
}

// Next Question
document.getElementById("next-btn").addEventListener("click", nextQuestion);

function nextQuestion() {
    currentQuestion++;
    if (currentQuestion < quizData.length) {
        loadQuestion();
    } else {
        alert("🎉 Quiz Completed! Final Score: " + score);
        document.getElementById("question-box").innerText = "Game Over!";
        document.getElementById("options-box").innerHTML = "";
        document.getElementById("next-btn").style.display = "none";
    }
}

// Start First Question
loadQuestion();
