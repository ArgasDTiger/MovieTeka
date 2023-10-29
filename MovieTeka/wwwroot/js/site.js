//Movie Actions
const actorsUl = document.getElementById("actorsUl");
actorsInput = document.getElementById("actorsInput");

tags = [];
createTag();
function createTag(){
    console.log("test");
    actorsUl.querySelectorAll("li").forEach(li => li.remove());
    tags.slice().reverse().forEach(tag =>{
        let liTag = `<li>${tag} <i class="uit uit-multiply" onclick="remove(this, '${tag}')"></i></li>`;
        actorsUl.insertAdjacentHTML("afterbegin", liTag);
    });
}

function remove(element, tag){
    let index = tags.indexOf(tag);
    tags = [...tags.slice(0, index), ...tags.slice(index + 1)];
    element.parentElement.remove();
}

function addTag(e) {
    if (e.key === "Enter") {
        e.preventDefault();
        let tag = e.target.value.replace(/\s+/g, ' ');
        if (tag.length > 1 && !tags.includes(tag)) {
            if (tags.length < 10) {
                tag.split(',').forEach(tag => {
                    tags.push(tag);
                    createTag();
                });
            }
        }
        e.target.value = "";
    }
    // Set the value of the hidden input field to the stringified tags array
    document.getElementById('actorsArray').value = JSON.stringify(tags);
}


actorsInput.addEventListener("keyup", addTag);

