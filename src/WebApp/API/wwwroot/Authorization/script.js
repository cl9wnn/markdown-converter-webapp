const createInputFieldWithError = (type, name, placeholder) => {
    const wrapper = createElement('div', { className: 'input-wrapper' });

    const input = createElement('input', {
        type,
        name,
        placeholder,
        required: true,
        className: 'input-field'
    });

    const errorSpan = createElement('span', {
        className: 'error-message',
        textContent: '',
    });

    wrapper.appendChild(input);
    wrapper.appendChild(errorSpan);

    return { wrapper, input, errorSpan };
};

const validateField = (input, errorSpan, rules) => {
    const value = input.value.trim();
    for (const rule of rules) {
        if (!rule.validate(value)) {
            errorSpan.textContent = rule.message;
            return false;
        }
    }
    errorSpan.textContent = '';
    return true;
};

const emailRules = [
    {
        validate: value => value.length > 0,
        message: 'Email is required.'
    },
    {
        validate: value => /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value),
        message: 'Invalid email address format.'
    }
];

const passwordRules = [
    {
        validate: value => value.length > 0,
        message: 'Password is required.'
    },
    {
        validate: value => /^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/.test(value),
        message: 'Password must be between 8 and 20 characters, include at least one digit, special symbol, and uppercase letter.'
    }
];

const firstNameRules = [
    {
        validate: value => value.length > 0,
        message: 'First name is required.'
    },
    {
        validate: value => /^[a-zA-Z0-9_\s]{5,20}$/.test(value),
        message: 'First name must be between 5 and 20 characters.'
    }
];

export const createLoginForm = (onClose) => {
    const form = createElement('form');

    const { wrapper: emailWrapper, input: emailInput, errorSpan: emailError } = createInputFieldWithError('text', 'email', 'Введите вашу почту');
    const { wrapper: passwordWrapper, input: passwordInput, errorSpan: passwordError } = createInputFieldWithError('password', 'password', 'Введите пароль');

    const submitButton = createElement('button', {
        type: 'submit',
        textContent: 'Войти',
        className: 'submit-button'
    });

    [emailWrapper, passwordWrapper, submitButton].forEach(el => form.appendChild(el));

    form.addEventListener('submit', async (event) => {
        event.preventDefault();

        const isEmailValid = validateField(emailInput, emailError, emailRules);
        const isPasswordValid = validateField(passwordInput, passwordError, passwordRules);

        if (!isEmailValid || !isPasswordValid) return;

        const formData = Object.fromEntries(new FormData(form).entries());
        const isSuccess = await handleSubmit('/auth/login', formData, true);
        onClose(isSuccess); 
    });

    return form;
};

export const createRegisterForm = (onClose) => {
    const form = createElement('form');

    const { wrapper: emailWrapper, input: emailInput, errorSpan: emailError } = createInputFieldWithError('text', 'email', 'Введите вашу почту');
    const { wrapper: firstNameWrapper, input: firstNameInput, errorSpan: firstNameError } = createInputFieldWithError('text', 'firstName', 'Введите ваше имя');
    const { wrapper: passwordWrapper, input: passwordInput, errorSpan: passwordError } = createInputFieldWithError('password', 'password', 'Введите пароль');

    const submitButton = createElement('button', {
        type: 'submit',
        textContent: 'Зарегистрироваться',
        className: 'submit-button'
    });

    [emailWrapper, firstNameWrapper, passwordWrapper, submitButton].forEach(el => form.appendChild(el));

    form.addEventListener('submit', async (event) => {
        event.preventDefault();

        const isEmailValid = validateField(emailInput, emailError, emailRules);
        const isFirstNameValid = validateField(firstNameInput, firstNameError, firstNameRules);
        const isPasswordValid = validateField(passwordInput, passwordError, passwordRules);

        if (!isEmailValid || !isFirstNameValid || !isPasswordValid) return;

        const formData = Object.fromEntries(new FormData(form).entries());
        const isSuccess = await handleSubmit('/auth/register', formData, true);
        onClose(isSuccess); 
    });

    return form;
};
export const tokenStorage = {
    save: (token) => localStorage.setItem('jwtToken', token),
    get: () => localStorage.getItem('jwtToken'),
    remove: () => localStorage.removeItem('jwtToken')
};

export const createElement = (tag, options = {}) => {
    const element = document.createElement(tag);
    Object.entries(options).forEach(([key, value]) => {
        if (key === 'className') element.className = value;
        else if (key === 'textContent') element.textContent = value;
        else if (key.startsWith('on')) element.addEventListener(key.slice(2).toLowerCase(), value);
        else element[key] = value;
    });
    return element;
};

const createOverlay = () => createElement('div', { className: 'overlay' });

const createPopup = (onClose) => {
    const popup = createElement('div', { className: 'popup' });
    const closeButton = createElement('button', {
        textContent: '×',
        className: 'close-button',
        onClick: () => {
            window.location.href = '/';
            document.body.removeChild(document.querySelector('.overlay'));
            if (onClose) onClose();
        }
    });
    popup.appendChild(closeButton);
    return popup;
};

export const showForm = (createFormMethod) => {
    return new Promise((resolve) => {
        const existingOverlay = document.querySelector('.overlay');
        if (existingOverlay) {
            document.body.removeChild(existingOverlay);
        }
        const overlay = createOverlay();
        const popup = createPopup(() => {
            document.body.removeChild(overlay);
            resolve({ success: false, message: 'Popup closed by user' });
        });

        const form = createFormMethod(async (isSuccess) => {
            document.body.removeChild(overlay);
            if (isSuccess) {
                resolve({ success: true, message: 'Operation completed successfully' });
            } else {
                resolve({ success: false, message: 'Operation failed' });
            }
        });

        popup.appendChild(form);
        overlay.appendChild(popup);
        document.body.appendChild(overlay);
    });
};

export function parseJwtToSub(token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(
            atob(base64)
                .split('')
                .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                .join('')
        );
        const playload = JSON.parse(jsonPayload);
        return playload.Sub || null;
    } catch (error) {
        console.error('Invalid token', error);
        return null;
    }
}

export function parseJwtToRole(token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(
            atob(base64)
                .split('')
                .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
                .join('')
        );
        const payload = JSON.parse(jsonPayload);
        return payload.Role || null;
    } catch (error) {
        console.error('Invalid token', error);
        return null;
    }
}

const handleSubmit = async (path, data, shouldHandleToken = false) => {
    try {
        const response = await fetch(path, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        if (!response.ok) {
            const responseData = await response.json();
            throw new Error(responseData.error || 'Ошибка при выполнении запроса');
        }

        if (shouldHandleToken) {
            const responseData = await response.json();
            const { token } = responseData;
            tokenStorage.save(token);
        }

        return true; 
    } catch (error) {
        console.error(error);
        return false; 
    }
};