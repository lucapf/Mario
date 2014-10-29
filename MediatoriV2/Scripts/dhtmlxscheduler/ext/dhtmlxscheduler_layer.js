/*
dhtmlxScheduler v.4.1.0 Stardard

This software is covered by GPL license. You also can obtain Commercial or Enterprise license to use it in non-GPL project - please contact sales@dhtmlx.com. Usage without proper license is prohibited.

(c) Dinamenta, UAB.
*/
scheduler.attachEvent("onTemplatesReady",function(){this.layers.sort(function(e,t){return e.zIndex-t.zIndex}),scheduler._dp_init=function(e){e._methods=["_set_event_text_style","","changeEventId","deleteEvent"],this.attachEvent("onEventAdded",function(t){!this._loading&&this.validId(t)&&this.getEvent(t)&&this.getEvent(t).layer==e.layer&&e.setUpdated(t,!0,"inserted")}),this.attachEvent("onBeforeEventDelete",function(t){if(this.getEvent(t)&&this.getEvent(t).layer==e.layer){if(!this.validId(t))return;
var s=e.getState(t);return"inserted"==s||this._new_event?(e.setUpdated(t,!1),!0):"deleted"==s?!1:"true_deleted"==s?!0:(e.setUpdated(t,!0,"deleted"),!1)}return!0}),this.attachEvent("onEventChanged",function(t){!this._loading&&this.validId(t)&&this.getEvent(t)&&this.getEvent(t).layer==e.layer&&e.setUpdated(t,!0,"updated")}),e._getRowData=function(e){var t=this.obj.getEvent(e),s={};for(var i in t)0!==i.indexOf("_")&&(s[i]=t[i]&&t[i].getUTCFullYear?this.obj.templates.xml_format(t[i]):t[i]);return s},e._clearUpdateFlag=function(){},e.attachEvent("insertCallback",scheduler._update_callback),e.attachEvent("updateCallback",scheduler._update_callback),e.attachEvent("deleteCallback",function(e,t){this.obj.setUserData(t,this.action_param,"true_deleted"),this.obj.deleteEvent(t)
})},function(){var e=function(t){if(null===t||"object"!=typeof t)return t;var s=new t.constructor;for(var i in t)s[i]=e(t[i]);return s};scheduler._dataprocessors=[],scheduler._layers_zindex={};for(var t=0;t<scheduler.layers.length;t++){if(scheduler.config["lightbox_"+scheduler.layers[t].name]={},scheduler.config["lightbox_"+scheduler.layers[t].name].sections=e(scheduler.config.lightbox.sections),scheduler._layers_zindex[scheduler.layers[t].name]=scheduler.config.inital_layer_zindex||5+3*t,scheduler.layers[t].url){var s=new dataProcessor(scheduler.layers[t].url);
s.layer=scheduler.layers[t].name,scheduler._dataprocessors.push(s),scheduler._dataprocessors[t].init(scheduler)}scheduler.layers[t].isDefault&&(scheduler.defaultLayer=scheduler.layers[t].name)}}(),scheduler.showLayer=function(e){this.toggleLayer(e,!0)},scheduler.hideLayer=function(e){this.toggleLayer(e,!1)},scheduler.toggleLayer=function(e,t){var s=this.getLayer(e);s.visible="undefined"!=typeof t?!!t:!s.visible,this.setCurrentView(this._date,this._mode)},scheduler.getLayer=function(e){var t,s;"string"==typeof e&&(s=e),"object"==typeof e&&(s=e.layer);
for(var i=0;i<scheduler.layers.length;i++)scheduler.layers[i].name==s&&(t=scheduler.layers[i]);return t},scheduler.attachEvent("onBeforeLightbox",function(e){var t=this.getEvent(e);return this.config.lightbox.sections=this.config["lightbox_"+t.layer].sections,scheduler.resetLightbox(),!0}),scheduler.attachEvent("onClick",function(e){var t=scheduler.getEvent(e);return!scheduler.getLayer(t.layer).noMenu}),scheduler.attachEvent("onEventCollision",function(e,t){var s=this.getLayer(e);if(!s.checkCollision)return!1;
for(var i=0,r=0;r<t.length;r++)t[r].layer==s.name&&t[r].id!=e.id&&i++;return i>=scheduler.config.collision_limit}),scheduler.addEvent=function(e,t,s,i,r){var a=e;1!=arguments.length&&(a=r||{},a.start_date=e,a.end_date=t,a.text=s,a.id=i,a.layer=this.defaultLayer),a.id=a.id||scheduler.uid(),a.text=a.text||"","string"==typeof a.start_date&&(a.start_date=this.templates.api_date(a.start_date)),"string"==typeof a.end_date&&(a.end_date=this.templates.api_date(a.end_date)),a._timed=this.isOneDayEvent(a);
var n=!this._events[a.id];this._events[a.id]=a,this.event_updated(a),this._loading||this.callEvent(n?"onEventAdded":"onEventChanged",[a.id,a])},this._evs_layer={};for(var e=0;e<this.layers.length;e++)this._evs_layer[this.layers[e].name]=[];scheduler.addEventNow=function(e,t,s){var i={};"object"==typeof e&&(i=e,e=null);var r=6e4*(this.config.event_duration||this.config.time_step);e||(e=Math.round(scheduler._currentDate().valueOf()/r)*r);var a=new Date(e);if(!t){var n=this.config.first_hour;n>a.getHours()&&(a.setHours(n),e=a.valueOf()),t=e+r
}i.start_date=i.start_date||a,i.end_date=i.end_date||new Date(t),i.text=i.text||this.locale.labels.new_event,i.id=this._drag_id=this.uid(),i.layer=this.defaultLayer,this._drag_mode="new-size",this._loading=!0,this.addEvent(i),this.callEvent("onEventCreated",[this._drag_id,s]),this._loading=!1,this._drag_event={},this._on_mouse_up(s)},scheduler._t_render_view_data=function(e){if(this.config.multi_day&&!this._table_view){for(var t=[],s=[],i=0;i<e.length;i++)e[i]._timed?t.push(e[i]):s.push(e[i]);this._table_view=!0,this.render_data(s),this._table_view=!1,this.render_data(t)
}else this.render_data(e)},scheduler.render_view_data=function(){if(this._not_render)return void(this._render_wait=!0);this._render_wait=!1,this.clear_view(),this._evs_layer={};for(var e=0;e<this.layers.length;e++)this._evs_layer[this.layers[e].name]=[];for(var t=this.get_visible_events(),e=0;e<t.length;e++)this._evs_layer[t[e].layer]&&this._evs_layer[t[e].layer].push(t[e]);if("month"==this._mode){for(var s=[],e=0;e<this.layers.length;e++)this.layers[e].visible&&(s=s.concat(this._evs_layer[this.layers[e].name]));
this._t_render_view_data(s)}else for(var e=0;e<this.layers.length;e++)if(this.layers[e].visible){var i=this._evs_layer[this.layers[e].name];this._t_render_view_data(i)}},scheduler._render_v_bar=function(e,t,s,i,r,a,n,d,l){var o=e.id;-1==n.indexOf("<div class=")&&(n=scheduler.templates["event_header_"+e.layer]?scheduler.templates["event_header_"+e.layer](e.start_date,e.end_date,e):n),-1==d.indexOf("<div class=")&&(d=scheduler.templates["event_text_"+e.layer]?scheduler.templates["event_text_"+e.layer](e.start_date,e.end_date,e):d);
var h=document.createElement("DIV"),_="dhx_cal_event",c=scheduler.templates["event_class_"+e.layer]?scheduler.templates["event_class_"+e.layer](e.start_date,e.end_date,e):scheduler.templates.event_class(e.start_date,e.end_date,e);c&&(_=_+" "+c);var u='<div event_id="'+o+'" class="'+_+'" style="position:absolute; top:'+s+"px; left:"+t+"px; width:"+(i-4)+"px; height:"+r+"px;"+(a||"")+'">';return u+='<div class="dhx_header" style=" width:'+(i-6)+'px;" >&nbsp;</div>',u+='<div class="dhx_title">'+n+"</div>",u+='<div class="dhx_body" style=" width:'+(i-(this._quirks?4:14))+"px; height:"+(r-(this._quirks?20:30))+'px;">'+d+"</div>",u+='<div class="dhx_footer" style=" width:'+(i-8)+"px;"+(l?" margin-top:-1px;":"")+'" ></div></div>',h.innerHTML=u,h.style.zIndex=100,h.firstChild
},scheduler.render_event_bar=function(e){var t=this._els.dhx_cal_data[0],s=this._colsS[e._sday],i=this._colsS[e._eday];i==s&&(i=this._colsS[e._eday+1]);var r=this.xy.bar_height,a=this._colsS.heights[e._sweek]+(this._colsS.height?this.xy.month_scale_height+2:2)+e._sorder*r,n=document.createElement("DIV"),d=e._timed?"dhx_cal_event_clear":"dhx_cal_event_line",l=scheduler.templates["event_class_"+e.layer]?scheduler.templates["event_class_"+e.layer](e.start_date,e.end_date,e):scheduler.templates.event_class(e.start_date,e.end_date,e);
l&&(d=d+" "+l);var o='<div event_id="'+e.id+'" class="'+d+'" style="position:absolute; top:'+a+"px; left:"+s+"px; width:"+(i-s-15)+"px;"+(e._text_style||"")+'">';e._timed&&(o+=scheduler.templates["event_bar_date_"+e.layer]?scheduler.templates["event_bar_date_"+e.layer](e.start_date,e.end_date,e):scheduler.templates.event_bar_date(e.start_date,e.end_date,e)),o+=scheduler.templates["event_bar_text_"+e.layer]?scheduler.templates["event_bar_text_"+e.layer](e.start_date,e.end_date,e):scheduler.templates.event_bar_text(e.start_date,e.end_date,e)+"</div>)",o+="</div>",n.innerHTML=o,this._rendered.push(n.firstChild),t.appendChild(n.firstChild)
},scheduler.render_event=function(e){var t=scheduler.xy.menu_width;if(scheduler.getLayer(e.layer).noMenu&&(t=0),!(e._sday<0)){var s=scheduler.locate_holder(e._sday);if(s){var i=60*e.start_date.getHours()+e.start_date.getMinutes(),r=60*e.end_date.getHours()+e.end_date.getMinutes()||60*scheduler.config.last_hour,a=Math.round((60*i*1e3-60*this.config.first_hour*60*1e3)*this.config.hour_size_px/36e5)%(24*this.config.hour_size_px)+1,n=Math.max(scheduler.xy.min_event_height,(r-i)*this.config.hour_size_px/60)+1,d=Math.floor((s.clientWidth-t)/e._count),l=e._sorder*d+1;
e._inner||(d*=e._count-e._sorder);var o=this._render_v_bar(e.id,t+l,a,d,n,e._text_style,scheduler.templates.event_header(e.start_date,e.end_date,e),scheduler.templates.event_text(e.start_date,e.end_date,e));if(this._rendered.push(o),s.appendChild(o),l=l+parseInt(s.style.left,10)+t,a+=this._dy_shift,o.style.zIndex=this._layers_zindex[e.layer],this._edit_id==e.id){o.style.zIndex=parseInt(o.style.zIndex)+1;var h=o.style.zIndex;d=Math.max(d-4,scheduler.xy.editor_width);var o=document.createElement("DIV");
o.setAttribute("event_id",e.id),this.set_xy(o,d,n-20,l,a+14),o.className="dhx_cal_editor",o.style.zIndex=h;var _=document.createElement("DIV");this.set_xy(_,d-6,n-26),_.style.cssText+=";margin:2px 2px 2px 2px;overflow:hidden;",_.style.zIndex=h,o.appendChild(_),this._els.dhx_cal_data[0].appendChild(o),this._rendered.push(o),_.innerHTML="<textarea class='dhx_cal_editor'>"+e.text+"</textarea>",this._quirks7&&(_.firstChild.style.height=n-12+"px"),this._editor=_.firstChild,this._editor.onkeypress=function(e){if((e||event).shiftKey)return!0;
var t=(e||event).keyCode;t==scheduler.keys.edit_save&&scheduler.editStop(!0),t==scheduler.keys.edit_cancel&&scheduler.editStop(!1)},this._editor.onselectstart=function(e){return(e||event).cancelBubble=!0,!0},_.firstChild.focus(),this._els.dhx_cal_data[0].scrollLeft=0,_.firstChild.select()}if(this._select_id==e.id){o.style.zIndex=parseInt(o.style.zIndex)+1;for(var c=this.config["icons_"+(this._edit_id==e.id?"edit":"select")],u="",v=0;v<c.length;v++)u+="<div class='dhx_menu_icon "+c[v]+"' title='"+this.locale.labels[c[v]]+"'></div>";
var g=this._render_v_bar(e.id,l-t+1,a,t,20*c.length+26,"","<div class='dhx_menu_head'></div>",u,!0);g.style.left=l-t+1,g.style.zIndex=o.style.zIndex,this._els.dhx_cal_data[0].appendChild(g),this._rendered.push(g)}}}},scheduler.filter_agenda=function(e,t){var s=scheduler.getLayer(t.layer);return s&&s.visible}});
//# sourceMappingURL=../sources/ext/dhtmlxscheduler_layer.js.map