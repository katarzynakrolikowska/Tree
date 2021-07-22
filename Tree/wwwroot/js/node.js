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

            $('#nodes').append(element);

            $('.js-node-item').on("click", function () {
                $(this).siblings('div').toggle();
            });

            $('.js-remove-btn').on("click", function () {
                let name = $(this).siblings('.js-node-name').text();

                if (confirm('Are you sure you want to remove node ' + name)) {
                    let id = $(this).attr('data-id');

                    removeNode(id);
                }
            });
        }
    });
});



function setNodes(node, element) {
    let expandCollapseBtn = $('<button type="button" class=" ml-4 btn btn-link js-node-item">').html('Expand/Collapse');
    let addBtn = $('<a href="#" class="ml-4 btn btn-link">').html('Add child node').attr('href', 'Node/Create/' + node.id);

    let editBtn = $('<a href="#" class="ml-4 btn btn-link">').html('Edit node').attr('href', 'Node/Edit/' + node.id);
    let removeBtn = $('<a href="javascript:;" class="ml-4 btn btn-link js-remove-btn">').html('Remove node').attr('data-id', node.id);

    let name = $('<span class="js-node-name">').html(node.name);
    let item = $('<div class="ml-4">').append(name).attr("id", node.id).append(addBtn).append(editBtn).append(removeBtn);

    if (node.parentNodeId != null) {
        
    }

    if (node.childNodes.length > 0) {
        item = item.append(expandCollapseBtn);
    }

    element.append(item);

    $.each(node.childNodes, function (i, childNode) {
        setNodes(childNode, element.find('#' + childNode.parentNodeId));
    });
}

function removeNode(id) {
    $.ajax({
        url: "/Node/Delete/" + id,
        type: "DELETE",
        contentType: "application/json",
        success: function (data) {
            console.log(data.success)

            $('#' + id).remove();

            if ($('#nodes').length) {
                location.reload();
            }
        }
    });
}