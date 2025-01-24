import {createAccessModal} from "./accessModal.js";
import {giveAccess, documentId} from "./script.js";

export function createAccessSettingsModal(permissionArray, clearPermission) {
    console.log(permissionArray);

    const modal = document.createElement('div');
    modal.className = 'per-modal';

    const modalContent = document.createElement('div');
    modalContent.className = 'per-modal-content';

    const modalHeader = document.createElement('div');
    modalHeader.className = 'per-modal-header';

    const title = document.createElement('h2');
    title.textContent = 'Users, who have access';

    const addButton = document.createElement('button');
    addButton.className = 'per-add-button';
    addButton.textContent = '+';
    addButton.onclick = async () => {
        modal.remove();
        await createAccessModal('Give access', async (inputData) => {
            const success = await giveAccess(inputData, documentId);
            if (success) {
                permissionArray.push({
                    email: inputData.email,
                    permissionName: inputData.permissionName,
                    documentId: inputData.documentId,
                });
                renderRoles();
            } else {
                console.error('Ошибка добавления роли');
            }
        });
    };

    const closeModalButton = document.createElement('button');
    closeModalButton.className = 'per-close-modal';
    closeModalButton.textContent = '×';

    closeModalButton.onclick = () => {
        modal.remove();
    };

    modalHeader.appendChild(title);
    modalHeader.appendChild(addButton); 
    modalHeader.appendChild(closeModalButton);

    const modalBody = document.createElement('div');
    modalBody.className = 'per-modal-body';

    function renderRoles() {
        modalBody.innerHTML = '';

        if (permissionArray.length === 0) {
            const noPermissionsMessage = document.createElement('div');
            noPermissionsMessage.className = 'per-no-permissions';
            noPermissionsMessage.textContent = 'Permissions not found';
            modalBody.appendChild(noPermissionsMessage);
            return;
        }

        permissionArray.forEach((role, index) => {
            const listItem = document.createElement('div');
            listItem.className = 'per-list-item';

            const roleInfo = document.createElement('span');
            roleInfo.textContent = `${role.email} - ${role.permissionName}`;

            const changeButton = document.createElement('button');
            changeButton.className = 'per-change-button';
            changeButton.textContent = 'change';
            changeButton.onclick = async () => {
                modal.remove();
                await createAccessModal('Give access', async (inputData) => {
                    await giveAccess(inputData, role.documentId);
                }, role.email);
            };

            const deleteButton = document.createElement('button');
            deleteButton.className = 'per-delete-button';
            deleteButton.textContent = '✖';
            deleteButton.onclick = async () => {
                const success = await clearPermission(role.documentId, role.email);
                if (success) {
                    permissionArray = permissionArray.filter((_, i) => i !== index);
                    renderRoles();
                } else {
                    console.error('Ошибка удаления роли');
                }
            };

            listItem.appendChild(roleInfo);
            listItem.appendChild(changeButton);
            listItem.appendChild(deleteButton);
            modalBody.appendChild(listItem);
        });
    }

    modalContent.appendChild(modalHeader);
    modalContent.appendChild(modalBody);
    modal.appendChild(modalContent);
    document.body.appendChild(modal);

    renderRoles();
}