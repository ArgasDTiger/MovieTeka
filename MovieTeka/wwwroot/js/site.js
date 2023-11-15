//Movie Actions
const actorsUl = document.getElementById("actorsUl");
actorsInput = document.getElementById("actorsInput");
function createTag(){
    let liTag = `<li>${tags[tags.length-1]} <i class="uit uit-multiply" onclick="remove(this, '${tags[tags.length-1]}')"></i></li>`;
    actorsUl.insertAdjacentHTML("afterbegin", liTag);
}

function remove(element, tag) {
    window.tags = window.tags.filter(t => t.trim() !== tag.trim());
    element.parentElement.remove();
    document.getElementById('actorsArray').value = JSON.stringify(window.tags);
}


function addTag(e) {
    if (e.key === "Enter") {
        e.preventDefault();
        let tag = e.target.value.replace(/\s+/g, ' ');
        if (tag.length > 1 && !window.tags.includes(tag)) {
            if (window.tags.length < 10) {
                tag.split(',').forEach(tag => {
                    window.tags.push(tag);
                    createTag();
                });
            }
        }
        e.target.value = "";
    }
    // Set the value of the hidden input field to the stringified window.tags array
    console.log(`tags1 = ${window.tags}`);
    document.getElementById('actorsArray').value = JSON.stringify(window.tags);
    console.log(`tags2 = ${window.tags}`);
}


actorsInput.addEventListener("keyup", addTag);