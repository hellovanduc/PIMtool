let searchString = document.getElementsByName("searchString")[0]

let btn_search = document.getElementById("btn_search")

let project_table_body = document.getElementById("project_table_body")
let project_table_rows = project_table_body.rows
let row_checkboxes = document.querySelectorAll("#project_table_body tr td input")
let list_delete_btn = document.querySelectorAll(".btn_delete")

let project_table_foot = document.getElementById("project_table_foot")
let num_of_selected = document.getElementById("num_of_selected")
let btn_delete_selected_items = document.getElementById("btn_delete_selected_items")
let form_delete = document.getElementById("form_delete")
let project_numbers_to_delete = document.getElementsByName("project_numbers")[0]

window.onload = function () {
    setOnclickCheckBoxes();
    setOnclickBtnDelete();
    btn_delete_selected_items.onclick = () => doDeleteSelectedItems()
    searchString.onkeyup = (event) => { 
        //  User press ENTER, do search
        if (event.keyCode === 13) btn_search.click()
    }
}

/*
    Set onclick handler event for all checkboxes in the project list table
*/
function setOnclickCheckBoxes() {
    row_checkboxes.forEach(checkbox => {
        checkbox.onclick = function () {
            if (checkbox.checked === true) {
                //  When a checkbox is checked: 
                //  - the number of selected checkboxes will be increased
                //  - footer (which contains the notification about selected checkboxes and delete selected checkboxes) will be displayed (if it is being hidden)
                increaseNumOfSelectedItems()
            }
            else {
                //  When a checkbox is unchecked: 
                //  - the number of selected checkboxes will be decreased 
                //  - footer (which contains the notification about selected checkboxes and delete selected checkboxes) will be hidden if the number of selected checkboxes equal to 0
                decreaseNumOfSelectedItems()
            }
        }
    });
}

function increaseNumOfSelectedItems() {
    //  Increase the number of selected checkboxes
    num_of_selected.innerText = parseInt(num_of_selected.innerText) + 1;

    //  Footer (which contains the notification about selected checkboxes and delete selected checkboxes)
    //      will be displayed (if it is being hidden)
    project_table_foot.hidden = false;
}

function decreaseNumOfSelectedItems() {
    //  Decrease the number of selected checkboxes
    num_of_selected.innerText = parseInt(num_of_selected.innerText) - 1;

    //  Footer (which contains the notification about selected checkboxes and delete selected checkboxes)
    //      will be hidden if the number of selected checkboxes equal to 0
    if (num_of_selected.innerText == 0)
        project_table_foot.hidden = true;
}

/*
    Set onclick handler event for all delete buttons on each row of the project list table
*/
function setOnclickBtnDelete() {
    list_delete_btn.forEach(btn => {
        btn.onclick = function () {
            //  Find the row which contains that button
            row = btn.parentNode.parentNode.parentNode

            //  Show modal to confirm deletion
            let project_number = row.childNodes[3].innerText;
            $('#deleteModal').modal()
                .find('#project_number_to_delete')
                .text(project_number + ' ?');
            $('#deleteModal').modal('show');

            //  If the deletion is confirm, delete this row
            $('#deleteModal .modal-footer .btn-primary').on('click', () => {
                project_numbers_to_delete.value = project_number;
                form_delete.submit();
            })
        }
    })
}

//  This function is onclick event handler of "button" delete selected items
function doDeleteSelectedItems() {
    //  Show modal to confirm deletion
    $('#deleteModal').modal('show');

    //  List all project number have been chose
    let list_project_number = []

    let length = project_table_rows.length
    for (let i = 0; i < length; i++) {
        if (project_table_rows[i].childNodes[1].childNodes[0].checked) {
            list_project_number.push(project_table_rows[i].childNodes[3].innerText);
        }
    }

    //  Notify user which project number has been chose
    $('#deleteModal').modal()
        .find('#project_number_to_delete')
        .text(list_project_number.toString() + ' ?');


    //  If the deletion is confirmed, delete the selected row(s)
    $('#deleteModal .modal-footer .btn-primary').on('click', () => {
        project_numbers_to_delete.value = list_project_number.toString();
        form_delete.submit();
    })
}
