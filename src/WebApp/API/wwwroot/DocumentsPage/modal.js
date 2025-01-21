export function createModal(titleText, onSave) {
    const modal = document.createElement('div');
    modal.className = 'modal';

    const modalOverlay = document.createElement('div');
    modalOverlay.className = 'modal-overlay';

    const modalContent = document.createElement('div');
    modalContent.className = 'modal-content';

    const modalTitle = document.createElement('h2');
    modalTitle.textContent = titleText;

    const titleInput = document.createElement('input');
    titleInput.type = 'text';
    titleInput.id = 'modalInput';
    titleInput.placeholder = 'Enter value';
    titleInput.className = 'modal-input';

    const saveButton = document.createElement('button');
    saveButton.textContent = 'Save';
    saveButton.className = 'modal-save-button';
    saveButton.addEventListener('click', async () => {
        const value = titleInput.value.trim();
        if (value) {
            await onSave(value);
            closeModal(modal, modalOverlay);
        } else {
            alert('Please enter a value.');
        }
    });

    const closeIcon = document.createElement('button');
    closeIcon.className = 'modal-close-icon';
    closeIcon.innerHTML = '&times;';
    closeIcon.addEventListener('click', () => {
        closeModal(modal, modalOverlay);
    });

    modalContent.appendChild(modalTitle);
    modalContent.appendChild(titleInput);
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