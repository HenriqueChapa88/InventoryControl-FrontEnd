document.getElementById('low-stock-btn').addEventListener('click', async () => {
    const threshold = prompt('Enter low stock threshold:', '5');
    if (threshold === null) return;

    try {
        const response = await fetch(`/api/items/low-stock/${threshold}`, {
            headers: getAuthHeader()
        });
        
        if (!response.ok) throw new Error('Failed to get low stock items');
        
        const items = await response.json();
        displayResults('Low Stock Items', items.map(i => 
            `${i.name} (${i.quantity} in stock)`));
    } catch (error) {
        alert(error.message);
    }
});

document.getElementById('inventory-value-btn').addEventListener('click', async () => {
    try {
        const response = await fetch('/api/items/inventory-value', {
            headers: getAuthHeader()
        });
        
        if (!response.ok) throw new Error('Failed to calculate inventory value');
        
        const value = await response.json();
        displayResults('Total Inventory Value', `$${value.toFixed(2)}`);
    } catch (error) {
        alert(error.message);
    }
});

function displayResults(title, content) {
    const results = document.getElementById('inventory-results');
    
    if (Array.isArray(content)) {
        results.innerHTML = `<h3>${title}</h3><ul>${
            content.map(item => `<li>${item}</li>`).join('')
        }</ul>`;
    } else {
        results.innerHTML = `<h3>${title}</h3><p>${content}</p>`;
    }
}