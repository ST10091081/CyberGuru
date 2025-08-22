// Parse questions injected by server (camelCase: id, text, options)
const QUESTIONS = JSON.parse(document.getElementById("quiz-data").textContent);

// State
let idx = 0;
let selected = new Array(QUESTIONS.length).fill(-1);
let score = 0;
let timeLeft = 15;
let timer;
const perQuestionSeconds = 15;

// Elements
const startBtn = document.getElementById("start-btn");
const intro = document.getElementById("intro");
const quiz = document.getElementById("quiz");
const summary = document.getElementById("summary");

const qBox = document.getElementById("question-box");
const optBox = document.getElementById("options-box");
const feedback = document.getElementById("feedback");
const nextBtn = document.getElementById("next-btn");
const timerLabel = document.getElementById("time");
const scoreLabel = document.getElementById("score");
const qnum = document.getElementById("qnum");
const qtotal = document.getElementById("qtotal");
const progress = document.getElementById("progress");

qtotal.innerText = QUESTIONS.length.toString();
scoreLabel.innerText = "0";

startBtn?.addEventListener("click", () => {
    intro.style.display = "none";
    quiz.style.display = "";
    renderQuestion();
});

function renderQuestion() {
    const q = QUESTIONS[idx];
    qnum.innerText = (idx + 1).toString();
    qBox.innerText = q.text; // FIX: camelCase
    optBox.innerHTML = "";
    feedback.innerHTML = "";
    nextBtn.disabled = true;

    // If the DB stored 2-choice questions (Safe/Phishing), options length will be 2.
    // Otherwise fall back to however many options exist (up to 4).
    q.options.forEach((opt, i) => {
        const btn = document.createElement("button");
        btn.className = "btn btn-outline-primary d-block w-100 mb-2 text-start";
        btn.innerText = opt;
        btn.onclick = () => choose(i, btn);
        optBox.appendChild(btn);
    });

    // reset timer
    clearInterval(timer);
    timeLeft = perQuestionSeconds;
    timerLabel.innerText = timeLeft;
    timer = setInterval(tick, 1000);

    progress.innerText = `Question ${idx + 1} of ${QUESTIONS.length}`;
}

function tick() {
    timeLeft--;
    timerLabel.innerText = timeLeft;
    if (timeLeft <= 0) {
        clearInterval(timer);
        // no selection counts as wrong
        selected[idx] = selected[idx] === -1 ? 99 : selected[idx];
        showFeedback(null); // timeout
        nextBtn.disabled = false;
    }
}

function choose(i, btn) {
    // prevent multiple selection
    if (selected[idx] !== -1) return;

    selected[idx] = i;
    nextBtn.disabled = false;

    showFeedback(i);
}

function showFeedback(choiceIndex) {
    // Ask server later; client preview uses correct option unknown -> simple visual:
    // Temporarily highlight the clicked choice and inform user feedback
    // Better: color the selected button; if you have correct index on client, color accordingly.
    const buttons = [...optBox.querySelectorAll("button")];
    buttons.forEach(b => b.disabled = true);

    // Optional: if you want to expose correct index on client (not recommended),
    // add it into the payload. Here we just show generic guidance:
    if (choiceIndex === null) {
        feedback.className = "text-warning";
        feedback.innerText = "⏱️ Time’s up. Consider scanning for sender domain and link mismatches.";
        score -= 5; // preview score; server is authoritative anyway
    } else {
        // Neutral feedback + heuristic coaching
        const tips = [
            "Check if the sender domain matches the organization.",
            "Hover links to verify the domain before clicking.",
            "Watch for urgent language and requests for credentials.",
            "Poor grammar/spelling can be a red flag."
        ];
        feedback.className = "text-info";
        feedback.innerText = `Answer recorded. Tip: ${tips[Math.floor(Math.random() * tips.length)]}`;
        // preview points (server will compute final)
        score += 10;
    }
    score = Math.max(0, score);
    scoreLabel.innerText = score.toString();
}

nextBtn.addEventListener("click", () => {
    clearInterval(timer);
    idx++;
    if (idx < QUESTIONS.length) {
        renderQuestion();
    } else {
        finish();
    }
});

async function finish() {
    quiz.style.display = "none";
    summary.style.display = "";

    // Build payload for server-side scoring
    const payload = {
        questionIds: QUESTIONS.map(q => q.id), // FIX: camelCase
        selectedIndices: selected.map(s => (s < 0 ? 99 : s)),
        durationSeconds: QUESTIONS.length * perQuestionSeconds // rough duration; you can track per-question for more accuracy
    };

    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;
    const res = await fetch(window.location.pathname + "?handler=Submit", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "RequestVerificationToken": token
        },
        body: JSON.stringify(payload)
    });

    const result = await res.json();
    document.getElementById("summary-line").innerText =
        `Score: ${result.score} | Correct: ${result.correct}/${result.total}`;
}
