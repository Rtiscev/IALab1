let rows = document.querySelector('#m_Rows');
let cols = document.querySelector('#m_Columns');
let matrixDiv = document.querySelector('#Matrix');
let from_selectionsDiv = document.querySelector('#from_selections');
let on_selectionsDiv = document.querySelector('#on_selections');
let stepsDiv = document.querySelector('#steps');
let handDiv = document.querySelector('#hand');
let repliesDiv = document.querySelector('#replies');
let queryDiv = document.querySelector('#query');

let temp_holder = ''
let AND_OR_TREE = []
let blocks = []

rows.value = 4;
cols.value = 7;
let matrix = [];
handDiv.textContent = "Пусто"

async function GenerateMatrix() {
    matrixDiv.innerHTML = "";
    if (rows.value > 2 && cols.value > 2) {
        matrixDiv.style.setProperty("grid-template-rows", `repeat(${rows.value},auto)`);
        matrixDiv.style.setProperty("grid-template-columns", `repeat(${cols.value},auto)`);
        for (let i = 0; i < rows.value; i++) {
            for (let j = 0; j < cols.value; j++) {
                let Block = `<input id="m_el${j + i * rows.value}" type="text" maxlength="3" placeholder="0"></input>`;
                matrixDiv.innerHTML += Block;
            }
        }
    }
    else {
        alert("размеры не могут быть пустыми или ошибочными");
    }
}

async function putOn() {
    put_on(from_selectionsDiv.value, on_selectionsDiv.value);
}

async function FillMatrix() {
    matrix = []
    // console.log(matrixDiv.childElementCount);
    let iter = 0;
    if (matrixDiv.childElementCount > 4) {
        for (let i = 0; i < rows.value; i++) {
            tempik = []
            for (let j = 0; j < cols.value; j++) {
                let tempV = matrixDiv.childNodes[iter++].value;
                if (tempV == '')
                    tempik.push('0');
                else {
                    blocks[tempV] = { Row: i, Col: j };
                    tempik.push(tempV);
                    let optionFromElement = document.createElement("option");
                    let optionOnElement = document.createElement("option");
                    optionFromElement.text = tempV;
                    optionOnElement.text = tempV;
                    from_selectionsDiv.appendChild(optionFromElement);
                    on_selectionsDiv.appendChild(optionOnElement);
                }
            }
            matrix.push(tempik);
        }
    }
    else {
        alert("размеры не могут быть пустыми или ошибочными");
    }
}

async function UpdateMatrix() {
    let iter = 0;
    for (let i = 0; i < rows.value; i++) {
        for (let j = 0; j < cols.value; j++) {
            matrixDiv.children[iter++].value = matrix[i][j];
        }
    }
}

function ColorCell(i, j, color) {
    console.log(i, j, color);
    matrixDiv.children[i * cols.value + j].style.backgroundColor = color;
}
 

async function move(block1, block2, get_rid_of = false) {
    let PUT_ON = null;
    if (get_rid_of) {
        AND_OR_TREE.push(`избавляемся от ${temp_holder}`);
        let Block = `<p>избавляемся от ${temp_holder}</p>`;
        stepsDiv.innerHTML += Block;
        await delayedPromise(1000);

        for (let i = matrix.length - 1; i >= 0; i--) {
            if (temp_holder == null) {
                // alert("finished")
                break;
            }
            for (let j = 0; j < matrix[0].length; j++) {
                if (matrix[i][j] == 0) {
                    matrix[i][j] = temp_holder;
                    blocks[temp_holder] = { Row: i, Col: j };
                    PUT_ON = i == matrix.length - 1 ? "стол" : matrix[i + 1][j];
                    AND_OR_TREE.push(`ставим ${temp_holder} на ${PUT_ON}`);
                    let Block = `<p>ставим ${temp_holder} на ${PUT_ON}</p>`;
                    stepsDiv.innerHTML += Block;
                    await delayedPromise(1000);

                    temp_holder = null;
                    handDiv.textContent = "Пусто"
                    break;
                }
            }
        }
    } else {
        // alert("finished")
        AND_OR_TREE.push(`ставим ${block1} по верх ${block2}`);
        let Block = `<p>ставим ${block1} по верх ${block2}</p>`;
        stepsDiv.innerHTML += Block;
        await delayedPromise(1000);

        matrix[blocks[block2].Row - 1][blocks[block2].Col] = temp_holder;
    }

    // ASYNC STUFF
    let promise = new Promise((resolve, reject) => {
        setTimeout(() => resolve("done!"), 500)
    });
    let result = await promise;
    UpdateMatrix();

    put_on(block1, block2);
    // alert("finished")
}

async function put_on(block1, block2) {
    if (matrix[blocks[block2].Row - 1][blocks[block2].Col] != block1) {
        if (matrix[blocks[block2].Row - 1][blocks[block2].Col] != 0) {
            AND_OR_TREE.push(`чистим верхушку ${block2}`);
            let Block = `<p>чистим верхушку ${block2}</p>`;
            stepsDiv.innerHTML += Block;
            await delayedPromise(1000);

            grasp(matrix[blocks[block2].Row - 1][blocks[block2].Col]);
            move(block1, block2, get_rid_of = true);
            // alert("finished")
        } else if (matrix[blocks[block1].Row - 1][blocks[block1].Col] != 0) {
            for (let i = 0; i < rows.value; i++) {
                if (matrix[i][blocks[block1].Col] != 0) {
                    AND_OR_TREE.push(
                        `чистим верхушку ${matrix[i + 1][blocks[block1].Col]}` // Adjusted index
                    );
                    let Block = `<p>чистим верхушку ${matrix[i + 1][blocks[block1].Col]}</p>`;
                    stepsDiv.innerHTML += Block;
                    await delayedPromise(1000);

                    grasp(matrix[i][blocks[block1].Col]);
                    move(block1, block2, get_rid_of = true);
                    break;
                }
            }
            // alert("finished") //CLOSE
        } else {
            AND_OR_TREE.push(`ставим ${block1} на ${block2}`);
            let Block = `<p>ставим ${block1} на ${block2}</p>`;
            stepsDiv.innerHTML += Block;
            await delayedPromise(1000);

            grasp(block1);
            move(block1, block2, get_rid_of = false);
            // alert("finished")
        }
        // alert("finished")
    }
    else {
        // ACTUAL ENDING
        let AND_OR_TREE = []
        // alert("finished")
    }
    // alert("finished")
}

async function SubmitQuery() {
    console.log(queryDiv.value);
    let query = queryDiv.value;
    if (query.startsWith("Как ты")) {
        if (query.includes("освободил верхушку")) {
            query = query.replace("Как ты освободил верхушку", "");
            query = query.replace("?", "");
            query = query.replace(" ", "");
            let isFound = false;
            let iPos = 0;
            for (let i = 0; i < AND_OR_TREE.length; i++) {
                if (AND_OR_TREE[i] == `схватываем ${query}`) {
                    isFound = true;
                    iPos = i;
                }
            }
            if (isFound) {
                if (iPos == AND_OR_TREE.length + 1) {
                    console.log(AND_OR_TREE[iPos]);
                }
                else {
                    console.log(`Потому что я сделал действие "${AND_OR_TREE[iPos + 1]}"`);
                }
            }
        }
        else if (query.includes("избавился от")) {
            query = query.replace("Как ты избавился от", "");
            query = query.replace("?", "");
            query = query.replace(" ", "");
            let isFound = false;
            let iPos = 0;
            for (let i = 0; i < AND_OR_TREE.length; i++) {
                if (AND_OR_TREE[i] == `схватываем ${query}`) {
                    isFound = true;
                    iPos = i;
                }
            }
            if (isFound) {
                if (iPos == AND_OR_TREE.length + 1) {
                    console.log(AND_OR_TREE[iPos]);
                }
                else {
                    console.log(`Потому что я сделал действие "${AND_OR_TREE[iPos + 1]}"`);
                }
            }
            // not yet
            else if (query.includes("поставил")) {
                query = query.replace("Как ты поставил", "");
                query = query.replace("?", "");
                query = query.replace(" ", "");
                let isFound = false;
                let iPos = 0;
                for (let i = 0; i < AND_OR_TREE.length; i++) {
                    if (AND_OR_TREE[i] == `схватываем ${query}`) {
                        isFound = true;
                        iPos = i;
                    }
                }
                if (isFound) {
                    if (iPos == AND_OR_TREE.length + 1) {
                        console.log(AND_OR_TREE[iPos]);
                    }
                    else {
                        console.log(`Потому что я сделал действие "${AND_OR_TREE[iPos + 1]}"`);
                    }
                }
            }
        }
    }


}

async function delayedPromise(delayMs) {
    return new Promise((resolve, reject) => {
        setTimeout(() => resolve("done!"), delayMs);
    });
}