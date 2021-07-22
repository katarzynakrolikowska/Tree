$(document).ready(function () {

    let node;

    $.ajax({
        url: "/Node/GetAll",
        type: "GET",
        contentType: "application/json",
        success: function (data) {
            node = data.data;

            let element = $('<div>');
            setNodes(node, element);

            $('#nodes').append(element.html());

            setEvents(node);
        }
    });
});



function setNodes(node, element) {
    let addBtn = $('<a class="ml-4 btn btn-link">').html('Add child node').attr('href', 'Node/Create/' + node.id);

    let editBtn = $('<a class="ml-4 btn btn-link">').html('Edit node').attr('href', 'Node/Edit/' + node.id);
    let removeBtn = $('<a href="javascript:;" class="ml-4 btn btn-link js-remove-btn">').html('Remove node').attr('data-id', node.id);

    let name = $('<span class="js-node-name">').html(node.name);
    let item = $('<div class="ml-4">').append(name).attr("id", node.id).append(addBtn).append(editBtn).append(removeBtn);

    if (node.childNodes.length > 0) {
        let expandCollapseBtn = $('<button type="button" class=" ml-4 btn btn-link js-expand-collapse-btn">').html('Expand/Collapse');
        let sortBtn = $('<a href="javascript:;" class="ml-4 btn btn-link js-sort-asc-btn">').html('Sort childs').attr('data-id', node.id);

        item = item.append(expandCollapseBtn).append(sortBtn);
    }

    element.append(item);

    $.each(node.childNodes, function (i, childNode) {
        setNodes(childNode, element.find('#' + childNode.parentNodeId));
    });
}

function setEvents(node) {
    $('.js-expand-collapse-btn').unbind('click');
    $('.js-expand-collapse-btn').on("click", function () {
        $(this).siblings('div').toggle();
    });

    $('.js-remove-btn').unbind('click');
    $('.js-remove-btn').on("click", function () {
        let name = $(this).siblings('.js-node-name').text();

        if (confirm('Are you sure you want to remove node ' + name + '?')) {
            let id = $(this).attr('data-id');

            removeNode(id);
        }
    });

    $('.js-sort-asc-btn').unbind('click');
    $('.js-sort-asc-btn').on("click", function () {
        sortChilds(this, node, true);
    });

    $('.js-sort-desc-btn').unbind('click');
    $('.js-sort-desc-btn').on("click", function () {
        sortChilds(this, node, false);
    });
}


function removeNode(id) {
    $.ajax({
        url: "/Node/Delete/" + id,
        type: "DELETE",
        contentType: "application/json",
        success: function (data) {

            $('#' + id).remove();

            if ($('#nodes').length) {
                location.reload();
            }
        }
    });
}

function sortChilds(element, node, asc) {
    let id = $(element).attr('data-id');

    let selectedNode = searchChild(node, id);

    selectedNode.childNodes.sort(function (a, b) {
        if (asc === true) {
            return a.name.toLowerCase().localeCompare(b.name);
        }
        else {
            return (-1) * (a.name.toLowerCase().localeCompare(b.name));
        }
        
    });

    let elementDiv = $('<div>');
    setNodes(selectedNode, elementDiv);

    $('#' + id).replaceWith(elementDiv.html());


    if (asc === true) {
        $('#' + id + ' .js-sort-asc-btn').first().addClass('js-sort-desc-btn').removeClass('js-sort-asc-btn');
    }
    else {
        $('#' + id + ' .js-sort-desc-btn').first().addClass('js-sort-asc-btn').removeClass('js-sort-desc-btn');
    }

    setEvents(node);


}

function searchChild(node, id) {
    let search;

    if (node.id == id) {
        search = node;
    }
    else {
        let i = 0;
        do {
            if (node.childNodes[i]) {
                search = searchChild(node.childNodes[i], id);
            }
            
            i++;
        } while ((node.childNodes.length > i) && (!search));
    }

    return search;
}