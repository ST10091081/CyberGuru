document.addEventListener('DOMContentLoaded', function () {
    const input = document.getElementById('passwordInput');
    const feedback = document.getElementById('pwdFeedback');
    let score = 0;

    input.addEventListener('input', function () {
        const val = input.value;
        if (!val) {
            feedback.innerHTML = '';
            score = 0;
            return;
        }
        const r = zxcvbn(val);           // returns score 0..4
        score = r.score * 25;            // map 0..4 to 0..100 (example)
        feedback.innerHTML = `Strength: ${r.score}/4 <br/>${r.feedback.warning || ''} <br/> ${r.feedback.suggestions.join(', ') || ''}`;
    });

    document.getElementById('submitPwdScore').addEventListener('click', async function () {
        try {
            const res = await fetch('?handler=SubmitScore', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ score })
            });
            const data = await res.json();
            if (data.success) alert('Password score submitted! New XP: ' + data.newXP);
        } catch (err) {
            console.error(err);
            alert('Failed to submit password score.');
        }
    });
});
