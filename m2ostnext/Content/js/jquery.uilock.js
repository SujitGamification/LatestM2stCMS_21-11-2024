/*
 * uiLock 1.0 - UI locker plugin for jQuery
 *
 * Copyright (c) 2009 jQuery Howto
 *
 * Licensed under the GPL license:
 *   http://www.gnu.org/licenses/gpl.html
 *
 * URL:
 *   http://jquery-howto.blogspot.com
 *
 * Author URL:
 *   http://me.boo.uz
 *
 */
(function($) {
	$.extend({ 
		uiLock: function(content){
			if(content == 'undefined') content = '';
			$('<div></div>').attr('id', 'uiLockId').css({
				'position': 'absolute',
				'top': 0,
				'left': 0,
				'z-index': 1000,
				'opacity': 0.6,
				'width':'100%',
				'height':'10000px',
				'color':'white',
				'background-color':'black'
			}).html(content).appendTo('body');
		},
		uiUnlock: function(){
			$('#uiLockId').remove();
		}
	});
})(jQuery);