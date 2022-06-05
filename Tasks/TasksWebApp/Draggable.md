# Draggables using HTML, Javascript and CSS

Providing drag and drop support in HTML/JS/CSS provides excellent usability to any user interface.

This short tutorial will show you how to add draggable support to your application.

There are lots of tutorials on how to make an unordered list of list items drggable, by working with <ul> and <li> elements, however in a real work practical exercise, this is unlikely to fit nicely.

In this example, we are using bootstrap grid feature, where each card is set to 50% of the total width for desktop monitors, so we get 2 cards per row, and then for mobile, each card is 100% width. This does not convert well to an unordered list, we we need to work with what we have.

I assume we have a bootstrap element of `<div class="row" >` that wraps around our cards, and that each card is contained in a `<div>` such as the following:

```c#


            <div class="col-lg-6 py-2">
                <a asp-page="Details" asp-route-id="@item.ID" style="text-decoration:inherit; color:inherit;">
                    <div class="card active-box flex-row flex-wrap" style="min-width:22rem">

                        <div class="mx-auto align-self-center p-3"><i class="fa fa-solid fa-list-alt fa-4x home-icon"></i></div>
                        <div class="card-block flex-fill mx-auto p-2">
                            <h3 class="card-title">@item.Title </h3>
                            <p class="card-text" style="min-width:22rem">
                                @item.Description<br />
                            </p>

                        </div>
                    </div>
                </a>
            </div>
```


* Step 1: Add `draggable-list" id="draggable-list">` to the enclosing row. 
* Step 2: Add `draggable" id="item-@item.ID" ` to the `col-lg-6` iterator div.
* Step 3: Add the follow icon-grip to end of the `active-card` div.

```c#
<div class="mx-auto p-3 editmode-grip "><i class="fa fa-arrows fa-2x home-icon"></i></div>
```
* Step 4: Add the CSS style inside a @section Styles {} block

```c#

    <style>

        .over {
            border: 1px dashed;
            border-color: mediumaquamarine;
        }

        .draggable-list {
            list-style-type: none;
            padding: 0;
            margin: 0;
        }
            

            .draggable-list div.over .draggable {
                border: 1px dashed;
                border-color: aquamarine;
            }

            .draggable-list.editmode {
                border: 2px dashed;
                border-color: mediumaquamarine;
            }

        .editmode-grip {
            display: none;
        }
    </style>
```
* Step 5: Add the following Script to the end of the page in the scripts section:
```c#
<script>
        /* foreach element with draggable class:
         * +-> add a 'dragstart' listener
         *      'dragstart' saves the drag start index of the <li>
         *      it find the closest <li> by searching up the dom to the outer elements until it finds a <li>
         *      then gets the 'data-index' of that <li> element
         *
         * foreach <li> within the "draggable-list li" <ul>:
         *  +-> add four event listeners: dragover, drop, dragenter, dragleave
         *      'dragover' had to prevent the default behaviour of dragging to open url or submit
         *      'drop' gets the data-index of the drop end index, and swaps elemtn with  start index
         *       and then removes the 'over' class from the drop end point element
         *      'dragenter' adds some ui sugar by adding the 'over' class to the entered element
         *      'dragleave' add soe UI sugar by removing the 'over' class from the left element
         *
         */

        const draggableList = document.getElementById('draggable-list');
        var isEditMode = false;

        init();

        function init() {

           
        }

   
        function edit() {

            isEditMode = !isEditMode;

            if (isEditMode) {
                console.log('Edit: ', "adding editmode");

                // make edit button solid
                var editButton = document.getElementById("edit-button");
                editButton.classList.remove("btn-outline-primary");
                editButton.classList.add("btn-primary");
                editButton.innerText = "Save";

                

                // add editmode class to draggable-list
                const dragList = document.getElementById('draggable-list');
                dragList.classList.add("editmode");

                // make grips visible
                grips = document.querySelectorAll('.editmode-grip');
                grips.forEach((grip) => {
                    grip.style.display = "block";
                });
                // add listeners 
                addEventListeners();
                
            }
            else {
                
                console.log('Edit: ', "removing editmode");

                // make eidt button outlined
                var editButton = document.getElementById("edit-button");                
                editButton.classList.remove("btn-primary");
                editButton.classList.add("btn-outline-primary");
                editButton.innerText = "Edit";


                // remove editmode class from draggble-list
                const dragList = document.getElementById('draggable-list');
                dragList.classList.remove("editmode");

                // hide grips 
                grips = document.querySelectorAll('.editmode-grip');
                grips.forEach((grip) => {
                    grip.style.display = "none";
                });

                // remove listeners
                removeEventListeners();

                // finally, sync with server
                syncOrder();

            }
            console.log('Edit: ', isEditMode);

        }

        function dragStart(ev) {
            ev.dataTransfer.setData("text", ev.target.id);
        }

        function dragOver(ev) {
            ev.preventDefault();
        }

        function dragDrop(ev) {

            ev.preventDefault();

            var sourceId = ev.dataTransfer.getData("text");
            var sourceElement = document.getElementById(sourceId);

            ev.target.closest(".draggable")
                .parentNode.insertBefore(sourceElement, ev.target.closest(".draggable"));

            this.classList.remove('over');

            // without this, the dragDrop event is fired 3 times
            ev.stopImmediatePropagation();

        }


        function dragEnter() {

            this.classList.add("over");
        }

        function dragLeave() {

            this.classList.remove('over');
        }


        function addEventListeners() {

            const draggables = document.querySelectorAll('.draggable');

            draggables.forEach((draggable) => {
                draggable.addEventListener('dragstart', dragStart);
                draggable.setAttribute('draggable', true);                
            });

            const dragListItems = document.querySelectorAll('.draggable-list div');

            dragListItems.forEach((item) => {
                item.addEventListener('dragover', dragOver);
                item.addEventListener('drop', dragDrop);
                item.addEventListener('dragenter', dragEnter);
                item.addEventListener('dragleave', dragLeave);
            });
        }

        function removeEventListeners() {

            const draggables = document.querySelectorAll('.draggable');

            draggables.forEach((draggable) => {
                draggable.removeEventListener('dragstart', dragStart);
                draggable.setAttribute('draggable', false);
            });

            const dragListItems = document.querySelectorAll('.draggable-list div');

            dragListItems.forEach((item) => {
                item.removeEventListener('dragover', dragOver);
                item.removeEventListener('drop', dragDrop);
                item.removeEventListener('dragenter', dragEnter);
                item.removeEventListener('dragleave', dragLeave);
            });
        }
        function getItems() {
            const elements = document.getElementsByClassName('draggable');
            let i = 0;
            var items = [...elements].map(function (x) {
                return {
                    id: x.id,
                    order: i++
                }
            });
           
            return items;
          
        }

        function syncOrder() {

            console.log('Event: ', 'syncOrder');

            items = getItems();
            console.log(items);
            //fetch('/SyncOrder', {
            //    method: 'PUT',
            //    headers: {
            //        'Accept': 'application/json',
            //        'Content-Type': 'application/json'
            //    },
            //    body: JSON.stringify(items)
            //})
            //    .then(() => getItems())
            //    .catch(error => console.error('Unable to sync items.', error));

            //closeInput();

            return false;
        }



    </script>
        <!-- End My Scripts-- >
```

* Step 6: Add a button below the 'Add' button
```c#
 <div class="col-lg-6 py-2 d-flex justify-content-end">
    <a class="btn btn-outline-primary" id="edit-button" onclick="edit()">Edit </a>
</div>
```

* Step 7: Write the javascript method to call the `syncOrder()` method that sends the correct order back to the server, obviouly also writing the backed Razor pages code to receive the correct order and save to databse.

* Step 8: Update the `Pages/Shared/_Layout.cshtml` to include a `STyles` section inside <head></head>
```html
  <!-- other head links...-->
  <link rel="stylesheet" href="~/TasksWebApp.styles.css" asp-append-version="true" />
    @await RenderSectionAsync("Styles", required: false)
</head>
```

# Errors:

* InvalidOperationException: The following sections have been defined but have not been rendered by the page at '/Pages/Shared/_Layout.cshtml': 'Styles'. To ignore an unrendered section call IgnoreSection("sectionName").

* Drag working but drop didn't work:

 Failed to execute 'insertBefore' on 'Node': parameter 1 is not of type 'Node'.
 
    var sourceId = ev.dataTransfer.getData("text");
    var sourceElement = document.getElementById(sourceId);

Turns out that the source taret is an <a> and not a <div>.
why?

Don't understand why it worked with static file draggables-ex.html but not with razor page

Fixed with getting .cloest("draggable");



# Issues
* only 1 draggable grid is allowed per page, if you have more than 1, you will need to refactor


# More Information:
* [Search Draggble support in the html standard](https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/draggable)

