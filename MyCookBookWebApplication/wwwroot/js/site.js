
function showAddTag() {
	var addTagArea = document.getElementById("addTag");
	if (addTagArea.style.display === "block") {
		addTagArea.style.display = "none";
	} else {
		addTagArea.style.display = "block";
	}
}

function showChangeRating() {
	var changeRatingArea = document.getElementById("changeRating");
	if (changeRatingArea.style.display === "block") {
		changeRatingArea.style.display = "none";
	} else {
		changeRatingArea.style.display = "block";
	}
}

function showDeleteTag() {
	var changeRatingArea = document.getElementById("deleteTag");
	if (changeRatingArea.style.display === "block") {
		changeRatingArea.style.display = "none";
	} else {
		changeRatingArea.style.display = "block";
	}
}

function deleteRecipe() {
	var deleteRecipeArea = document.getElementById("deleteRecipe");
	if (deleteRecipeArea.style.display === "block") {
		deleteRecipeArea.style.display = "none";
	} else {
		deleteRecipeArea.style.display = "block";
	}
}
