﻿/*! 
* DevExtreme (Vector Map)
* Version: 15.1.5
* Build date: Jul 15, 2015
*
* Copyright (c) 2012 - 2015 Developer Express Inc. ALL RIGHTS RESERVED
* EULA: https://www.devexpress.com/Support/EULAs/DevExtreme.xml
*/
(function(n,t){function a(){}function o(n){return n}function u(n){return typeof n=="function"}function y(n,t,u,f){var o=n[i]?tt(new e(n[i])):{},s=n[r]?ut(new e(n[r])):{},h;return o.errors&&o.errors.forEach(function(n){f.push("SHP: "+n)}),s.errors&&s.errors.forEach(function(n){f.push("DBF: "+n)}),h=p(o.shapes||[],s.records||[],t,u),h.length?{features:h,bbox:o.bbox,type:o.type}:null}function p(n,t,i,r){var f=[],u,o,e;for(f.length=o=Math.max(n.length,t.length),u=0;u<o;++u)e=n[u]||{},f[u]={coordinates:e.coordinates?i(e.coordinates):null,attributes:t[u]?r(t[u]):null};return f}function w(n,i){function r(n,t){n&&(e[n]=t),--u==0&&i(f,e)}var u=1,f=[],e={};Object.keys(n).forEach(function(i){var e=n[i];e!==t&&e!==null&&(++u,e.substr?ct(e,function(n,t){n&&f.push(n),r(i,t)}):r(i,e))}),r()}function b(n){function r(n){return Math.round(n*t)/t}function i(n){return n.map(n[0].length?i:r)}var t=Number("1E"+n);return i}function k(n){function r(n){var r={};return Object.keys(n).forEach(function(u){var f=t[u]||i(u);f&&(r[f]=n[u])}),r}var t,i;try{t=JSON.parse(n)}catch(u){t={},n.split(";").forEach(function(n){var i=n.split("=");t[i[0]]=i[1]||!0})}return i=t.$clear?function(){return null}:t.$lowercase?function(n){return n.toLowerCase()}:t.$uppercase?function(n){return n.toUpperCase()}:o,r}function d(n){var t;return t=u(n.processCoordinates)?n.processCoordinates:n.precision>=0?b(n.precision):o}function g(n){var t;return t=u(n.processAttributes)?n.processAttributes:n.attrsMap?k(n.attrsMap):o}function nt(n,t,f){n=n||{},f=u(t)&&t||u(f)&&f||a,t=!u(t)&&t||{};var e={},o;return n.substr?n.substr(-i.length).toLowerCase()===i?e[i]=n:n.substr(-r.length).toLowerCase()===r?e[r]=n:(e[i]=n+i,e[r]=n+r):(e[i]=n.shp,e[r]=n.dbf),w(e,function(n,i){var r=[];n&&n.forEach(function(n){r.push("URI: "+n)}),o=y(i,d(t),g(t),r),f(o,r.length?r:null)}),o}function tt(n){var f=new Date,t,i=[],u=[],r;try{t=it(n)}catch(e){return i.push("Terminated: "+e.message+" / "+e.description),{errors:i}}t.fileCode!==9994&&i.push("File code: "+t.fileCode+" / expected: 9994"),t.version!==1e3&&i.push("File version: "+t.version+" / expected: 1000");try{while(n.position()<t.fileLength)if(r=rt(n,t.shapeType,i),r)u.push(r);else{i.push("Terminated");break}n.position()!==t.fileLength&&i.push("File length: "+t.fileLength+" / actual: "+n.position())}catch(e){i.push("Terminated: "+e.message+" / "+e.description)}finally{return{bbox:[t.bbox.xmin,t.bbox.ymin,t.bbox.xmax,t.bbox.ymax],type:s[t.shapeType],shapes:u,errors:i,time:new Date-f}}}function it(n){return{fileCode:n.uint32(!0),unused1:n.uint32(!0),unused2:n.uint32(!0),unused3:n.uint32(!0),unused4:n.uint32(!0),unused5:n.uint32(!0),fileLength:n.uint32(!0)<<1,version:n.uint32(),shapeType:n.uint32(),bbox:{xmin:n.float64(),ymin:n.float64(),xmax:n.float64(),ymax:n.float64(),zmin:n.float64(),zmax:n.float64(),mmin:n.float64(),mmax:n.float64()}}}function rt(n,t,i){var r={},u,f,e;return(r.number=n.uint32(!0),u=n.uint32(!0)<<1,f=n.position(),r.shapeType=n.uint32(),r.shapeName=s[r.shapeType],!r.shapeName)?(i.push("Shape #"+r.number+" type: "+r.shapeType+" / unknown"),null):(r.shapeType!==t&&i.push("Shape #"+r.number+" type: "+r.shapeName+" / expected: "+s[t]),h[r.shapeType](n,r),e=n.position(),e-f!==u&&i.push("Shape #"+r.number+" length: "+u+" / actual: "+e-f),r)}function ut(n){var f=new Date,t=[],i,r,u;try{i=ft(n,t),r=st(i,t),u=ht(n,i.numberOfRecords,i.recordLength,r,t)}catch(e){t.push("Terminated: "+e.message+" / "+e.description)}finally{return{records:u,errors:t,time:new Date-f}}}function ft(n,t){var i={versionNumber:n.uint8(),lastUpdate:new Date(1900+n.uint8(),n.uint8()-1,n.uint8()),numberOfRecords:n.uint32(),headerLength:n.uint16(),recordLength:n.uint16()},f,r,u;for(n.skip(20),f=(i.headerLength-n.position()-1)/32,r=[];f-->0;)r.push(et(n));return i.fields=r,u=n.uint8(),u!==13&&t.push("Header terminator: "+u+" / expected: 13"),i}function f(n,t){return c.apply(null,n.uint8array(t))}function et(n){var t={name:f(n,11).replace(/\0*$/gi,""),type:c(n.uint8()),length:n.skip(4).uint8(),count:n.uint8()};return n.skip(14),t}function ot(n,t){return n.skip(t),null}function st(n,t){for(var e=[],u=0,o=n.fields.length,r,i,f=0;u<o;++u)i=n.fields[u],r={name:i.name,parser:v[i.type],length:i.length},r.parser||(r.parser=ot,t.push("Field "+i.name+" type: "+i.type+" / unknown")),f+=i.length,e.push(r);return f+1!==n.recordLength&&t.push("Record length: "+n.recordLength+" / actual: "+(f+1)),e}function ht(n,t,i,r,u){for(var o=0,f,a=r.length,s,h,l=[],c,e;o<t;++o){for(c={},s=n.position(),n.skip(1),f=0;f<a;++f)e=r[f],c[e.name]=e.parser(n,e.length);h=n.position(),h-s!==i&&u.push("Record #"+(o+1)+" length: "+i+" / actual: "+h-s),l.push(c)}return l}function e(n){this._dataView=new DataView(n),this._position=0}function ct(n,t){var i=new XMLHttpRequest;i.onreadystatechange=function(){this.readyState===4&&(this.statusText==="OK"?t(null,this.response):t(this.statusText,null))},i.open("GET",n),i.responseType="arraybuffer",i.setRequestHeader("X-Requested-With","XMLHttpRequest"),i.send(null)}var i=".shp",r=".dbf",s={0:"null",1:"point",3:"polyline",5:"polygon",8:"multipoint"},h={0:a,1:function(n,t){t.coordinates=[n.float64(),n.float64()]},3:function(n,t){for(var l=[n.float64(),n.float64(),n.float64(),n.float64()],s=n.uint32(),f=n.uint32(),r=[],h=[],u,e,c=[],o,i=0;i<s;++i)r.push(n.uint32());for(i=0;i<f;++i)h.push([n.float64(),n.float64()]);for(i=0;i<s;++i){for(u=r[i],e=r[i+1]||f,o=[],u=r[i],e=r[i+1]||f;u<e;++u)o.push(h[u]);c.push(o)}t.bbox=l,t.coordinates=c},8:function(n,t){for(var u=[n.float64(),n.float64(),n.float64(),n.float64()],f=n.uint32(),r=[],i=0;i<f;++i)r.push([n.float64(),n.float64()]);t.bbox=u,t.coordinates=r}},c,v,l;h[5]=h[3],c=String.fromCharCode,v={C:function(n,t){var i=f(n,t);return i.trim()},N:function(n,t){var i=f(n,t);return parseFloat(i,10)},D:function(n,t){var i=f(n,t);return new Date(i.substring(0,4),i.substring(4,6)-1,i.substring(6,8))}},e.prototype={constructor:e,position:function(){return this._position},skip:function(n){return this._position+=n,this},uint8array:function(n){for(var i=this._dataView,r=0,t=[];r++<n;)t.push(i.getUint8(this._position++));return t},uint8:function(){return this._dataView.getUint8(this._position++)},uint16:function(n){var t=this._dataView.getUint16(this._position,!n);return this._position+=2,t},uint32:function(n){var t=this._dataView.getUint32(this._position,!n);return this._position+=4,t},float64:function(n){var t=this._dataView.getFloat64(this._position,!n);return this._position+=8,t}},l=n.DevExpress=n.DevExpress||{},(l.viz=l.viz||{}).vectormaputils={parse:nt}})(window);