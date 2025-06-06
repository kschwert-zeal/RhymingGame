let questionId;
let questionNumber = 0;

window.onload = function () {
    var countdown = document.getElementById('countdown');
    var gameContent = document.getElementById('gameContent');
    var count = 3;
    countdown.style.display = 'block';

    var interval = setInterval(function () {
        count--;
        countdown.innerText = count;
        if (count === 0) {
            clearInterval(interval);
            countdown.style.display = 'none';
            gameContent.style.display = 'block';
            getNextQuestion();
        }
    }, 1000);
};

getNextQuestion = () => {
    $.ajax({
        url: '/Home/GetNextQuestion', // URL of the action
        type: 'GET',
        data: { questionNumber: questionNumber }, // Send questionNumber as a parameter
        success: function (response) {
            // Handle the response (GameQuestions object)
            if (response) {
                $('#question-title').text('Question ' + (questionNumber + 1));
                $('#question-text').text(response.clue); // Assuming 'clue' is a property in GameQuestions
                questionId = response.gameQuestionId;
                questionNumber++;
            } else {
                $('#question-text').text('No questions available.');
            }
        },
        error: function (xhr, status, error) {
            console.error('Error fetching question:', error);
            $('#question-text').text('An error occurred while fetching the question.');
        }
    });
}

// Attach a keypress event listener to the input box
$('#answer').on('keypress', function (event) {
    if (event.key === 'Enter') {
        event.preventDefault(); // Prevent the default form submission behavior

        // Get the value of the input box
        let answer = document.getElementById('answer');

        if (answer.value.split(' ').length !== 2) {
            console.log('Please enter exactly two words separated by a space.');
        } else {
            $.ajax({
                url: '/Home/CheckAnswer', // URL of the action
                type: 'POST',
                data: { answer: answer.value, gameQuestionId: questionId }, // Send questionNumber as a parameter
                success: function (response) {
                    // Handle the response (GameQuestions object)
                    console.log(response);
                    let questionResult = document.getElementById('question-result');
                    questionResult.innerHTML = response.message;
                    if (questionNumber === 5) {
                        questionResult.innerHTML = "Game over!<br />";
                    }
                    else if (response.message !== 'Flip It') {
                        answer.value = '';
                        getNextQuestion();
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching question:', error);
                    $('#question-text').text('An error occurred while fetching the question.');
                }
            });
        }

        // Clear the input box after capturing the value
        //$(this).val('');
    }
});

function showLogin() {
    document.getElementById('login-form').classList.add('active');
    document.getElementById('register-form').classList.remove('active');
    document.querySelectorAll('.toggle-btn')[0].classList.add('active');
    document.querySelectorAll('.toggle-btn')[1].classList.remove('active');
    hideMessages();
}

function showRegister() {
    document.getElementById('register-form').classList.add('active');
    document.getElementById('login-form').classList.remove('active');
    document.querySelectorAll('.toggle-btn')[1].classList.add('active');
    document.querySelectorAll('.toggle-btn')[0].classList.remove('active');
    hideMessages();
}

function hideMessages() {
    document.getElementById('error-message').style.display = 'none';
    document.getElementById('success-message').style.display = 'none';
}

function showError(message) {
    const errorDiv = document.getElementById('error-message');
    errorDiv.textContent = message;
    errorDiv.style.display = 'block';
    document.getElementById('success-message').style.display = 'none';
}

function showSuccess(message) {
    const successDiv = document.getElementById('success-message');
    successDiv.textContent = message;
    successDiv.style.display = 'block';
    document.getElementById('error-message').style.display = 'none';
}

function validatePasswordStrength(password) {
    const requirements = {
        'req-length': password.length >= 8,
        'req-uppercase': /[A-Z]/.test(password),
        'req-lowercase': /[a-z]/.test(password),
        'req-number': /\d/.test(password),
        'req-special': /[^A-Za-z0-9]/.test(password)
    };

    Object.keys(requirements).forEach(id => {
        const element = document.getElementById(id);
        if (requirements[id]) {
            element.classList.add('valid');
        } else {
            element.classList.remove('valid');
        }
    });

    return Object.values(requirements).every(req => req);
}

function validateLogin() {
    const email = document.getElementById('login-email').value;
    const password = document.getElementById('login-password').value;

    if (!email || !password) {
        showError('Please enter both email and password.');
        return false;
    }

    if (!isValidEmail(email)) {
        showError('Please enter a valid email address.');
        return false;
    }

    return true;
}

function validateRegistration() {
    const firstName = document.getElementById('register-firstname').value.trim();
    const lastName = document.getElementById('register-lastname').value.trim();
    const email = document.getElementById('register-email').value;
    const password = document.getElementById('register-password').value;
    const confirmPassword = document.getElementById('confirm-password').value;

    if (!firstName || !lastName) {
        showError('Please enter both first and last name.');
        return false;
    }

    if (!isValidEmail(email)) {
        showError('Please enter a valid email address.');
        return false;
    }

    if (!validatePasswordStrength(password)) {
        showError('Password does not meet the requirements.');
        return false;
    }

    if (password !== confirmPassword) {
        showError('Passwords do not match.');
        return false;
    }

    return true;
}

function isValidEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

// Add real-time password confirmation validation
document.addEventListener('DOMContentLoaded', function () {
    const confirmPasswordInput = document.getElementById('confirm-password');
    const passwordInput = document.getElementById('register-password');

    function checkPasswordMatch() {
        if (confirmPasswordInput.value && passwordInput.value) {
            if (confirmPasswordInput.value === passwordInput.value) {
                confirmPasswordInput.style.borderColor = '#10b981';
            } else {
                confirmPasswordInput.style.borderColor = '#ef4444';
            }
        }
    }

    confirmPasswordInput.addEventListener('input', checkPasswordMatch);
    passwordInput.addEventListener('input', checkPasswordMatch);
});