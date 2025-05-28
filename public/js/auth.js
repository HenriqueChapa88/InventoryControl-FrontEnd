let currentToken = null;

document.getElementById('login-form').addEventListener('submit', async function(e) {
    e.preventDefault();
    
    const email = document.getElementById('login-email').value;
    const password = document.getElementById('login-password').value;

    try {
        const response = await fetch('/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                email: email,
                password: password
            })
        });

        if (!response.ok) {
            throw new Error('Login failed');
        }

        const data = await response.json();
        currentToken = data.token;
        updateAuthUI(true);
        document.getElementById('login-modal').style.display = 'none';
        loadInitialData();
    } catch (error) {
        alert(error.message);
    }
});

document.getElementById('register-form').addEventListener('submit', async function(e) {
    e.preventDefault();
    
    const name = document.getElementById('register-name').value;
    const email = document.getElementById('register-email').value;
    const password = document.getElementById('register-password').value;
    const role = document.getElementById('register-role').value;

    try {
        const response = await fetch('/api/auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                name: name,
                email: email,
                password: password,
                role: role
            })
        });

        if (!response.ok) {
            throw new Error('Registration failed');
        }

        document.getElementById('register-modal').style.display = 'none';
        alert('Registration successful! Please login.');
    } catch (error) {
        alert(error.message);
    }
});

document.getElementById('logout-btn').addEventListener('click', function() {
    currentToken = null;
    updateAuthUI(false);
    clearData();
});

function updateAuthUI(isLoggedIn) {
    document.getElementById('login-btn').style.display = isLoggedIn ? 'none' : 'block';
    document.getElementById('register-btn').style.display = isLoggedIn ? 'none' : 'block';
    document.getElementById('logout-btn').style.display = isLoggedIn ? 'block' : 'none';
    
    if (isLoggedIn) {
        loadInitialData();
    }
}

function getAuthHeader() {
    return {
        'Authorization': `Bearer ${currentToken}`,
        'Content-Type': 'application/json'
    };
}

function loadInitialData() {
    loadSuppliers();
    loadItems();
}

function clearData() {
    document.querySelector('#suppliers-table tbody').innerHTML = '';
    document.querySelector('#items-table tbody').innerHTML = '';
}

document.querySelectorAll('.modal .close').forEach(btn => {
    btn.addEventListener('click', function() {
        this.closest('.modal').style.display = 'none';
    });
});

document.getElementById('login-btn').addEventListener('click', function() {
    document.getElementById('login-modal').style.display = 'block';
});

document.getElementById('register-btn').addEventListener('click', function() {
    document.getElementById('register-modal').style.display = 'block';
});

window.addEventListener('click', function(event) {
    if (event.target.classList.contains('modal')) {
        event.target.style.display = 'none';
    }
});