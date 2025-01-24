import {tokenStorage, showForm, createLoginForm} from "../Authorization/script.js";
import {createModal } from "./modal.js";

document.addEventListener('DOMContentLoaded', async () => {
    const projects = await getProjects();
    await loadProjectTable(projects);
});


document.getElementById("createBtn").addEventListener("click",  async () => {
        await createModal('Создать документ', async (title) => {
            await createDocument(title);
        });
});
export function loadProjectTable(projectArray) {
    const container = document.getElementById('projectTableContainer');
    container.innerHTML = '';  

    if (!Array.isArray(projectArray) || projectArray.length === 0) {
        container.textContent = 'No projects available.';
        return;
    }

    const table = document.createElement('table');
    table.setAttribute('border', '1');

    const headerRow = document.createElement('tr');
    const headers = ['Name', 'Created At', 'Actions'];

    headers.forEach(headerText => {
        const th = document.createElement('th');
        th.textContent = headerText;
        headerRow.appendChild(th);
    });
    table.appendChild(headerRow);

    projectArray.forEach(project => {
        const row = document.createElement('tr');

        const nameCell = document.createElement('td');
        nameCell.textContent = project.name;
        row.appendChild(nameCell);

        const dateCell = document.createElement('td');
        const date = new Date(project.createdAt).toLocaleString();
        dateCell.textContent = date;
        row.appendChild(dateCell);

        const actionsCell = document.createElement('td');

        const openButton = document.createElement('button');
        openButton.textContent = 'Open';
        openButton.addEventListener('click', () => openDocument(project.documentId));
        actionsCell.appendChild(openButton);

        const renameButton = document.createElement('button');
        renameButton.textContent = 'Rename';
        renameButton.addEventListener('click', () => {
            createModal('Изменить название', async (newTitle) => {
                await renameProject(newTitle, project.documentId);
            });
        });
        actionsCell.appendChild(renameButton);

        const deleteButton = document.createElement('button');
        deleteButton.textContent = 'Delete';
        deleteButton.addEventListener('click', () => deleteDocument(project.documentId));
        actionsCell.appendChild(deleteButton);

        row.appendChild(actionsCell);
        table.appendChild(row);
    });

    container.appendChild(table);
}

export async function getProjects() {
    let token = tokenStorage.get();

    try {
        const response = await fetch(`/api/documents/get`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
        });

        if (response.ok) {
            const responseData = await response.json();
            await loadProjectTable(responseData.projects);
            return responseData.projects;
        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm);
            if (loginSuccessful) {
                return await getProjects();
            }
        } else {
            const data = await response.json();
            alert(data.error);
            return data;
        }
    } catch (error) {
        alert(error);
        throw error;
    }
}

async function deleteDocument(documentId) {
    let token = tokenStorage.get();

    try {
        const response = await fetch(`/api/documents/${documentId}/delete`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            }
        });

        if (response.ok) {
            const projects = await getProjects();
            await loadProjectTable(projects);
        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm, '/auth/signin', 'Sign In');

            if (loginSuccessful) {
                await deleteDocument(documentId);
            }
        } else {
            const data = await response.json();
            alert(data.error);
        }
    } catch (error) {
        alert(error);
    }
}

async function openDocument(documentId) {
   window.location.href = `documents/${documentId}`;
}

async function createDocument(title) {
    let token = tokenStorage.get();

    try {
        const response = await fetch(`/api/documents/create`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(title)
        });

        if (response.ok) {
            const projects = await getProjects();
            await loadProjectTable(projects);
        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm, '/auth/signin', 'Sign In');

            if (loginSuccessful) {
                await createDocument(title);
            }
        } else {
            const data = await response.json();
            alert(data.error);
        }
    } catch (error) {
        alert(error);
    }
}

async function renameProject(newName, documentId) {

    let token = tokenStorage.get();

    try {
        const response = await fetch(`/api/documents/${documentId}/rename`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${token}`
            },
            body: JSON.stringify(newName)
        });

        if (response.ok) {
            const data = await response.json();
            const projects = await getProjects();
            await loadProjectTable(projects);

        } else if (response.status === 401) {
            const loginSuccessful = await showForm(createLoginForm, '/auth/signin', 'Sign In');

            if (loginSuccessful) {
                await renameProject(newName, documentId);
            }
        } else {
            const data = await response.json();
            alert(data.error);
        }
    } catch (error) {
        alert(error);
    }
}
