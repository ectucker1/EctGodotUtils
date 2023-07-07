async function startEditor() {
    const response = await fetch("/api/list");
    const liveValues = await response.json();
    console.log(liveValues);
    
    const changeSocket = new WebSocket("ws://" + location.host + "/changes");

    function renderUi(selectedCategory) {
        replaceCategoriesList(selectedCategory);
        selectCategory(selectedCategory);
    }

    function replaceCategoriesList(selectedCategory) {
        const categoriesList = document.getElementById('categories-list');
        categoriesList.replaceChildren(...Object.entries(liveValues).map((category) => {
            const categoryItem = document.createElement('h3');
            categoryItem.innerText = category[0];
            categoryItem.classList.add('cat-button');
            categoryItem.addEventListener('click', () => {
                renderUi(category[0]);
            });
            return categoryItem;
        }));
    }

    function selectCategory(selectedCategory) {
        const valuesTable = document.getElementById('values-table');
        while (valuesTable.lastElementChild !== valuesTable.firstElementChild) {
            valuesTable.removeChild(valuesTable.lastElementChild);
        }

        valuesTable.append(...liveValues[selectedCategory].map(value => {
            const valueRow = document.createElement('tr');

            const nameData = document.createElement('td');
            const nameHeader = document.createElement('h3');
            nameHeader.innerText = value.name;
            nameData.appendChild(nameHeader);

            const valueData = document.createElement('td');
            const valueEditor = createValueEditor(value);
            valueData.appendChild(valueEditor);

            valueRow.append(nameData, valueData);
            return valueRow;
        }));
    }

    function createValueEditor(value) {
        if (value['TypeId'].includes('Switch')) {
            return createSwitchEditor(value);
        } else if (value['TypeId'].includes('Range')) {
            return createRangeEditor(value);
        } else {
            return document.createElement('div');
        }
    }

    function createRangeEditor(value) {
        const valueEditor = document.createElement('div');

        const slider = document.createElement('input');
        slider.type = 'range';
        slider.min = value.min;
        slider.max = value.max;
        slider.step = 0.00001;
        slider.value = value.value;

        const numEditor = document.createElement('input');
        numEditor.type = 'number';
        numEditor.min = value.min;
        numEditor.max = value.max;
        numEditor.step = 0.00001;
        numEditor.value = value.value;

        slider.addEventListener('input', () => {
            numEditor.value = slider.value;
            sendUpdate(value.category, value.name, slider.value);
        });

        numEditor.addEventListener('input', () => {
            slider.value = numEditor.value;
            sendUpdate(value.category, value.name, numEditor.value);
        });

        valueEditor.append(slider, numEditor);

        return valueEditor;
    }

    function createSwitchEditor(value) {
        const valueEditor = document.createElement('div');
        const checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.checked = value.value;
        checkbox.addEventListener('input', () => {
            sendUpdate(value.category, value.name, checkbox.checked);
        });
        valueEditor.appendChild(checkbox);
        return valueEditor;
    }
    
    function sendUpdate(category, name, value) {
        var change = {
            'category': category,
            'name': name,
            'value': value
        };
        changeSocket.send(JSON.stringify(change));
    }

    const defaultSelect = Object.entries(liveValues)[0][0];
    renderUi(defaultSelect);
}

startEditor();
