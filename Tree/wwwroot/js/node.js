$(document).ready(function () {

    let node;

    $.ajax({
        url: "/Node/GetAll",
        type: "GET",
        contentType: "application/json",
        success: function (data) {
            node = data.data;
            console.log(node);

            let element = $('<div>');
            setNodes(node, element);

            $('#nodes').append(element);

            $('.js-node-item').on("click", function () {
                $(this).siblings('div').toggle();
            });
        }
    });
});



function setNodes(node, element) {
    let expandCollapseBtn = $('<button type="button" class=" ml-4 btn btn-link js-node-item">').html('Expand/Collapse');
    let addBtn = $('<button type="button" class=" ml-4 btn btn-link">').html('Add child node');
    let editBtn = $('<button type="button" class=" ml-4 btn btn-link">').html('Edit node');

    let item = $('<div class="ml-4">').html(node.name).attr("id", node.id).append(addBtn).append(editBtn);

    if (node.parentNodeId != null) {
        item = item.hide();
    }

    if (node.childNodes.length > 0) {
        item = item.append(expandCollapseBtn);
    }

    element.append(item);

    $.each(node.childNodes, function (i, childNode) {
        setNodes(childNode, element.find('#' + childNode.parentNodeId));
    });
}