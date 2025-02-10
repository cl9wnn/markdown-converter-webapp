export function openHistoryModal(data, func, documentId) {
    if (document.querySelector(".his-modal-overlay")) return;

    const overlay = document.createElement("div");
    overlay.classList.add("his-modal-overlay");

    const modal = document.createElement("div");
    modal.classList.add("his-modal");

    const closeButton = document.createElement("button");
    closeButton.innerHTML = "&times;";
    closeButton.classList.add("his-close-modal-btn");
    closeButton.addEventListener("click", () => overlay.remove());

    const clearHistoryButton = document.createElement("button");
    clearHistoryButton.classList.add("his-clear-history-btn");
    clearHistoryButton.addEventListener("click", async () => {
        const success = await func(documentId); 
        if (success) {
            tbody.innerHTML = ""; 
        }
    });

    modal.appendChild(closeButton);
    modal.appendChild(clearHistoryButton);

    const title = document.createElement("h2");
    title.textContent = "Журнал изменений";
    modal.appendChild(title);

    const table = document.createElement("table");
    table.classList.add("log-table");

    const thead = document.createElement("thead");
    const headerRow = document.createElement("tr");
    headerRow.innerHTML = `<th>Имя пользователя</th><th>Дата</th><th>Время</th>`;
    thead.appendChild(headerRow);
    table.appendChild(thead);

    const tbody = document.createElement("tbody"); 
    data.forEach(entry => {
        const { date, time } = formatDate(entry.date);
        const row = document.createElement("tr");
        row.innerHTML = `<td>${entry.userName}</td><td>${date}</td><td>${time}</td>`;
        tbody.appendChild(row);
    });
    table.appendChild(tbody);

    modal.appendChild(table);
    overlay.appendChild(modal);
    document.body.appendChild(overlay);

    overlay.addEventListener("click", (e) => {
        if (e.target === overlay) overlay.remove();
    });
}
function formatDate(isoString) {
    const date = new Date(isoString);

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");
    const hours = String(date.getHours()); 
    const minutes = String(date.getMinutes()).padStart(2, "0");

    return { date: `${year}-${month}-${day}`, time: `${hours}:${minutes}` };
}