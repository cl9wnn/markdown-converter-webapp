export function createAccessModal(titleText, onSave, defaultEmail = '') {
    const modal = document.createElement('div');
    modal.className = 'modal';

    const modalOverlay = document.createElement('div');
    modalOverlay.className = 'modal-overlay';

    const modalContent = document.createElement('div');
    modalContent.className = 'modal-content';

    const modalTitle = document.createElement('h2');
    modalTitle.textContent = titleText;

    const emailInput = document.createElement('input');
    emailInput.type = 'email';
    emailInput.id = 'emailInput';
    emailInput.placeholder = 'Enter your email';
    emailInput.className = 'modal-input';
    emailInput.value = defaultEmail; 

    const roleSelect = document.createElement('select');
    roleSelect.id = 'roleSelect';
    roleSelect.className = 'modal-select';

    const readerOption = document.createElement('option');
    readerOption.value = '1';
    readerOption.textContent = 'Reader';

    const editorOption = document.createElement('option');
    editorOption.value = '2';
    editorOption.textContent = 'Editor';
    

    roleSelect.appendChild(readerOption);
    roleSelect.appendChild(editorOption);

    const saveButton = document.createElement('button');
    saveButton.textContent = 'Save';
    saveButton.className = 'modal-save-button';
    saveButton.addEventListener('click', async () => {
        const email = emailInput.value.trim();
        const role = parseInt(roleSelect.value, 10);

        if (email && validateEmail(email)) {
            await onSave({ email, role });
            closeModal(modal, modalOverlay);
        } else {
            alert('Please enter a valid email.');
        }
    });

    const closeIcon = document.createElement('button');
    closeIcon.className = 'modal-close-icon';
    closeIcon.innerHTML = '&times;';
    closeIcon.addEventListener('click', () => {
        closeModal(modal, modalOverlay);
    });

    modalContent.appendChild(modalTitle);
    modalContent.appendChild(emailInput);
    modalContent.appendChild(roleSelect);
    modalContent.appendChild(saveButton);
    modal.appendChild(closeIcon);
    modal.appendChild(modalContent);

    document.body.appendChild(modalOverlay);
    document.body.appendChild(modal);
}

function closeModal(modal, overlay) {
    modal.remove();
    overlay.remove();
}

function validateEmail(email) {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}