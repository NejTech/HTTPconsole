$(document).ready(function() {
	var con = document.getElementById('content');

	var scrollBarWidth = getScrollBarWidth();
	var consoleWidth = $('#content').width();

	con.style.width = scrollBarWidth + consoleWidth;

	worker();
});

function worker() {
	var warn = document.getElementById('warning');
	var con = document.getElementById('content');
	$.ajax({ 
		url: 'raw' 
	}).done( function(data) {
		var isScrolled = con.scrollTop == con.scrollHeight - con.offsetHeight;
		con.innerHTML = data;
		if(isScrolled) {
    		con.scrollTop = con.scrollHeight;
    	}
	}).always( function() {
		setTimeout(worker, 200);
	}).fail( function() {
		warn.innerHTML = "WARNING: An error occured during communication with HTTPconsole server.";
	});
};

function getScrollBarWidth () {
	var $outer = $('<div>').css({'visibility':'hidden', 'width':'100', 'overflow':'scroll'}).appendTo('body'),
		widthWithScroll = $('<div>').css('width','100%').appendTo($outer).outerWidth();
		$outer.remove();
	return 100 - widthWithScroll;
};
