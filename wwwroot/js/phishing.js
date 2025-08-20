// phishing.js
// Minimal phishing spotter client: short list of sample emails and simple scoring
document.addEventListener('DOMContentLoaded', function () {

    // sample emails - replace or expand for more realistic training
    const emails = [
        { subject: 'Bank account alert', body: 'We detected suspicious activity. Click to verify: http://bank.example/verify', isPhishing: true },
        { subject: 'Team meeting notes', body: 'Attached the meeting notes from today.', isPhishing: false },
        { subject: 'Reset your password', body: 'Change your password at http://reset.example', isPhishing: true },
        { subject: 'Invoice from vendor', body: 'Please see attached invoice for services.', isPhishing: false }
    ];

    let score = 0;
    const container = document.getElementById('emailsContainer');
    const scoreEl = document.getElementById('score');

    // render "cards" for each email
    emails.forEach((email, i) => {
        const card = document.createElement('div');
        card.className = 'card my-2 p-3';
        card.innerHTML = `<h5>${email.subject}</h5><p>${email.body}</p>
      <button class="btn btn-danger me-2" data-index="${i}" data-choice="phishing">Phishing</button>
      <button class="btn btn-success" data-index="${i}" data-choice="legit">Legit</button>`;
        container.appendChild(card);
    });

    // click handler for all buttons
    container.addEventListener('click', function (e) {
        if (e.target.tagName !== 'BUTTON') return;
        const index = parseInt(e.target.getAttribute('data-index'));
        const choice = e.target.getAttribute('data-choice');
        const email = emails[index];

        const correct = (choice === 'phishing' && email.isPhishing) || (choice === 'legit' && !email.isPhishing);
        if (correct) {
            score += 10;
            e.target.classList.add('disabled');
            e.target.innerText = 'Correct';
        } else {
            score -= 5;
            e.target.classList.add('disabled');
            e.target.innerText = 'Wrong';
        }
        scoreEl.innerText = score;
    });

    // submit the score to the server
    document.getElementById('submitScoreBtn').addEventListener('click', async function () {
        try {
            const response = await fetch('?handler=SubmitScore', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ score })
            });
            const data = await response.json();
            if (data.success) {
                alert('Score submitted! Total XP: ' + data.newXP);
            } else {
                alert('Error saving score');
            }
        } catch (err) {
            console.error(err);
            alert('Failed to submit score (check console).');
        }
    });

});
