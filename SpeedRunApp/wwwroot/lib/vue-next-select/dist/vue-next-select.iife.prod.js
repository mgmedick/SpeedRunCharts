this.VueNextSelect=function(e){"use strict";function t(e){return(t="function"==typeof Symbol&&"symbol"==typeof Symbol.iterator?function(e){return typeof e}:function(e){return e&&"function"==typeof Symbol&&e.constructor===Symbol&&e!==Symbol.prototype?"symbol":typeof e})(e)}function n(e){return function(e){if(Array.isArray(e))return a(e)}(e)||function(e){if("undefined"!=typeof Symbol&&null!=e[Symbol.iterator]||null!=e["@@iterator"])return Array.from(e)}(e)||o(e)||function(){throw new TypeError("Invalid attempt to spread non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.")}()}function o(e,t){if(e){if("string"==typeof e)return a(e,t);var n=Object.prototype.toString.call(e).slice(8,-1);return"Object"===n&&e.constructor&&(n=e.constructor.name),"Map"===n||"Set"===n?Array.from(e):"Arguments"===n||/^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)?a(e,t):void 0}}function a(e,t){(null==t||t>e.length)&&(t=e.length);for(var n=0,o=new Array(t);n<t;n++)o[n]=e[n];return o}function l(e,t){var n="undefined"!=typeof Symbol&&e[Symbol.iterator]||e["@@iterator"];if(!n){if(Array.isArray(e)||(n=o(e))||t&&e&&"number"==typeof e.length){n&&(e=n);var a=0,l=function(){};return{s:l,n:function(){return a>=e.length?{done:!0}:{done:!1,value:e[a++]}},e:function(e){throw e},f:l}}throw new TypeError("Invalid attempt to iterate non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method.")}var r,i=!0,u=!1;return{s:function(){n=n.call(e)},n:function(){var e=n.next();return i=e.done,e},e:function(e){u=!0,r=e},f:function(){try{i||null==n.return||n.return()}finally{if(u)throw r}}}}var r={inheritAttrs:!1,name:"vue-input",props:{modelValue:{required:!0,type:String},placeholder:{required:!0,type:String},disabled:{required:!0,type:Boolean},tabindex:{required:!0,type:Number},autofocus:{required:!0,type:Boolean},comboboxUid:{required:!0,type:Number}},emits:["update:modelValue","input","change","focus","blur","escape"],setup:function(t,n){var o=e.ref(null);return e.onMounted((function(){t.autofocus&&o.value.focus()})),e.onUpdated((function(){t.autofocus&&o.value.focus()})),{handleInput:function(e){n.emit("input",e),n.emit("update:modelValue",e.target.value)},handleChange:function(e){n.emit("change",e),n.emit("update:modelValue",e.target.value)},handleFocus:function(e){n.emit("focus",e)},handleBlur:function(e){n.emit("blur",e)},input:o,handleEscape:function(e){o.value.blur(),n.emit("escape",e)}}}},i={class:"vue-input"};r.render=function(t,n,o,a,l,r){return e.openBlock(),e.createBlock("div",i,[e.renderSlot(t.$slots,"prepend"),e.createVNode("input",{ref:"input",modelValue:o.modelValue,placeholder:o.placeholder,disabled:o.disabled,onInput:n[1]||(n[1]=function(){return a.handleInput&&a.handleInput.apply(a,arguments)}),onChange:n[2]||(n[2]=function(){return a.handleChange&&a.handleChange.apply(a,arguments)}),onFocus:n[3]||(n[3]=function(){return a.handleFocus&&a.handleFocus.apply(a,arguments)}),onBlur:n[4]||(n[4]=function(){return a.handleBlur&&a.handleBlur.apply(a,arguments)}),onKeyup:n[5]||(n[5]=e.withKeys(e.withModifiers((function(){return a.handleEscape&&a.handleEscape.apply(a,arguments)}),["exact"]),["esc"])),tabindex:o.tabindex,autofocus:o.autofocus,"aria-autocomplete":"list","aria-controls":"vs".concat(o.comboboxUid,"-listbox"),"aria-labelledby":"vs".concat(o.comboboxUid,"-combobox")},null,40,["modelValue","placeholder","disabled","tabindex","autofocus","aria-controls","aria-labelledby"]),e.renderSlot(t.$slots,"append")])},r.__file="src/components/input.vue";var u={inheritAttrs:!1,name:"vue-tags",props:{modelValue:{required:!0,type:Array,validator:function(e){return e.every((function(e){return void 0!==t(e.key)&&void 0!==e.label&&"boolean"==typeof e.selected}))}},collapseTags:{type:Boolean}},emits:["click"],setup:function(t,n){return{dataAttrs:e.inject("dataAttrs"),handleClick:function(e){n.emit("click",e)}}}};u.render=function(t,n,o,a,l,r){return e.openBlock(),e.createBlock("ul",e.mergeProps({class:["vue-tags",{collapsed:o.collapseTags}],onMousedown:n[1]||(n[1]=e.withModifiers((function(){}),["prevent"])),tabindex:"-1",onClick:n[2]||(n[2]=function(){return a.handleClick&&a.handleClick.apply(a,arguments)})},a.dataAttrs),[(e.openBlock(!0),e.createBlock(e.Fragment,null,e.renderList(o.modelValue,(function(n){return e.openBlock(),e.createBlock(e.Fragment,{key:n.key},[n.group?e.createCommentVNode("v-if",!0):(e.openBlock(),e.createBlock("li",{key:0,class:["vue-tag",{selected:n.selected}]},[e.renderSlot(t.$slots,"default",{option:n},(function(){return[e.createVNode("span",null,e.toDisplayString(n.label),1)]}))],2))],64)})),128))],16)},u.__file="src/components/tags.vue";var c={inheritAttrs:!1,name:"vue-dropdown",props:{modelValue:{required:!0,type:Array,validator:function(e){return e.every((function(e){return void 0!==t(e.key)&&void 0!==e.label&&"boolean"==typeof e.selected}))}},comboboxUid:{required:!0,type:Number},maxHeight:{required:!0},highlightedOriginalIndex:{required:!0}},emits:["click-item","mouseenter"],setup:function(t,n){return{dataAttrs:e.inject("dataAttrs"),handleClickItem:function(e,t){t.disabled||n.emit("click-item",e,t)},handleMouseenter:function(e,t){n.emit("mouseenter",e,t)}}}};c.render=function(t,n,o,a,l,r){return e.openBlock(),e.createBlock("ul",e.mergeProps({class:"vue-dropdown",style:{maxHeight:o.maxHeight+"px"},onMousedown:n[1]||(n[1]=e.withModifiers((function(){}),["prevent"]))},a.dataAttrs,{role:"listbox",id:"vs".concat(o.comboboxUid,"-listbox"),"aria-multiselectable":a.dataAttrs["data-multiple"],"aria-busy":a.dataAttrs["data-loading"],"aria-disabled":a.dataAttrs["data-disabled"]}),[(e.openBlock(!0),e.createBlock(e.Fragment,null,e.renderList(o.modelValue,(function(n,l){return e.openBlock(),e.createBlock(e.Fragment,{key:n.key},[n.visible&&!1===n.hidden?(e.openBlock(),e.createBlock("li",{key:0,onClick:function(e){return a.handleClickItem(e,n)},class:["vue-dropdown-item",{selected:n.selected,disabled:n.disabled,highlighted:n.originalIndex===o.highlightedOriginalIndex,group:n.group}],onMouseenter:function(e){return a.handleMouseenter(e,n)},role:"option",id:"vs".concat(o.comboboxUid,"-option-").concat(l),"aria-selected":!!n.selected||!!n.disabled&&void 0,"aria-disabled":n.disabled},[e.renderSlot(t.$slots,"default",{option:n},(function(){return[e.createVNode("span",null,e.toDisplayString(n.label),1)]}))],42,["onClick","onMouseenter","id","aria-selected","aria-disabled"])):e.createCommentVNode("v-if",!0)],64)})),128))],16,["id","aria-multiselectable","aria-busy","aria-disabled"])},c.__file="src/components/dropdown.vue";var d=function(e,t,n){var o=n.valueBy;return o(e)===o(t)},s=function(e,t,n){var o=n.valueBy;return e.some((function(e){return d(e,t,{valueBy:o})}))},p=function(e,t,n){var o=n.valueBy;return e.find((function(e){return o(e)===t}))},v=function(e,t,n){var o=n.max,a=n.valueBy;return s(e,t,{valueBy:a})||e.length>=o?e:e.concat(t)},f=function(e,t,n){var o=n.min,a=n.valueBy;return!1===s(e,t,{valueBy:a})||e.length<=o?e:e.filter((function(e){return!1===d(e,t,{valueBy:a})}))},g=function(t){return e.computed((function(){return"function"==typeof t.value?t.value:"string"==typeof t.value?function(e){return t.value.split(".").reduce((function(e,t){return e[t]}),e)}:function(e){return e}}))};var h={name:"vue-select",inheritAttrs:!1,props:{modelValue:{required:!0},emptyModelValue:{default:null},options:{required:!0,type:Array},labelBy:{type:[String,Function]},valueBy:{type:[String,Function]},disabledBy:{default:"disabled",type:[String,Function]},groupBy:{default:"group",type:[String,Function]},visibleOptions:{type:[Array,null],default:null},multiple:{default:!1,type:Boolean},min:{default:0,type:Number},max:{default:1/0,type:Number},searchable:{default:!1,type:Boolean},searchPlaceholder:{default:"Type to search",type:String},clearOnSelect:{default:!1,type:Boolean},clearOnClose:{default:!1,type:Boolean},taggable:{default:!1,type:Boolean},collapseTags:{default:!1,type:Boolean},disabled:{default:!1,type:Boolean},loading:{default:!1,type:Boolean},closeOnSelect:{default:!1,type:Boolean},hideSelected:{default:!1,type:Boolean},placeholder:{default:"Select option",type:String},tabindex:{default:0,type:Number},autofocus:{default:!1,type:Boolean},maxHeight:{default:300,type:Number},openDirection:{type:String,validator:function(e){return["top","bottom"].includes(e)}}},emits:["selected","removed","update:modelValue","focus","blur","toggle","opened","closed","search:input","search:change","search:focus","search:blur"],setup:function(t,o){var a=function(t){var n=e.reactive({}),o=g(e.toRef(t,"labelBy"));e.watchEffect((function(){return n.labelBy=o.value}));var a=g(e.toRef(t,"valueBy"));e.watchEffect((function(){return n.valueBy=a.value}));var l=g(e.toRef(t,"disabledBy"));e.watchEffect((function(){return n.disabledBy=l.value}));var r=g(e.toRef(t,"groupBy"));e.watchEffect((function(){return n.groupBy=r.value}));var i=e.computed((function(){return t.multiple?t.min:Math.min(1,t.min)}));e.watchEffect((function(){return n.min=i.value}));var u=e.computed((function(){return t.multiple?t.max:1}));return e.watchEffect((function(){return n.max=u.value})),e.watchEffect((function(){return n.options=t.options})),n}(t),r=e.getCurrentInstance(),i=e.ref(),u=e.ref(),c=e.ref(),d=e.computed((function(){var e;return null===(e=c.value)||void 0===e?void 0:e._.refs.input})),h=e.ref(!1);e.watch((function(){return h.value}),(function(){h.value?(o.emit("opened"),o.emit("focus"),t.searchable?(d.value!==document.activeElement&&d.value.focus(),o.emit("search:focus")):i.value.focus()):(t.searchable?(d.value===document.activeElement&&d.value.blur(),t.clearOnClose&&O(),o.emit("search:blur")):i.value.blur(),o.emit("closed"),o.emit("blur")),o.emit("toggle")}));var m=function(){t.disabled||(h.value=!0)},y=function(e){i.value.contains(null==e?void 0:e.relatedTarget)?setTimeout((function(){i.value.focus()})):h.value=!1};e.watch((function(){return t.disabled}),(function(){return y()}));var b=e.ref(""),B=e.computed((function(){return new RegExp(b.value.replace(/[.*+?^${}()|[\]\\]/g,"\\$&"),"i")})),w=e.computed((function(){return b.value?a.options.filter((function(e){return B.value.test(a.labelBy(e))})):void 0})),I=e.ref([]),x=e.computed((function(){return new Set(I.value.map((function(e){return a.valueBy(e)})))})),k=function(){if(t.multiple){if(!1===Array.isArray(t.modelValue))return!1;if(I.value.length!==t.modelValue.length)return!1;if(Object.keys(I.value).some((function(e){return I.value[e]!==p(a.options,t.modelValue[e],{valueBy:a.valueBy})})))return!1}else{if(0===I.value.length&&t.modelValue!==t.emptyModelValue)return!1;if(1===I.value.length&&t.modelValue===t.emptyModelValue)return!1;if(I.value[0]!==p(a.options,t.modelValue,{valueBy:a.valueBy}))return!1}return!0},V=function(){if(!k()){I.value=[];var e,n=l(t.multiple?t.modelValue:t.modelValue===t.emptyModelValue?[]:[t.modelValue]);try{for(n.s();!(e=n.n()).done;){var o=e.value,r=p(a.options,o,{valueBy:a.valueBy});!1!==s(a.options,r,{valueBy:a.valueBy})&&(I.value=v(I.value,r,{max:1/0,valueBy:a.valueBy}))}}catch(e){n.e(e)}finally{n.f()}}};V(),e.watch((function(){return t.modelValue}),(function(){return V()}),{deep:!0}),e.watch((function(){return a.options}),(function(){I.value=a.options.filter((function(e){return x.value.has(a.valueBy(e))}))}),{deep:!0});var M,N=function(e,t){(t=t.originalOption).value.every((function(e){var t=p(a.options,e,{valueBy:a.valueBy});return s(I.value,t,{valueBy:a.valueBy})}))?t.value.forEach((function(e){var t=p(a.options,e,{valueBy:a.valueBy});I.value=f(I.value,t,{min:a.min,valueBy:a.valueBy}),o.emit("removed",t)})):t.value.forEach((function(e){var t=p(a.options,e,{valueBy:a.valueBy});s(I.value,t,{valueBy:a.valueBy})||(I.value=v(I.value,t,{max:a.max,valueBy:a.valueBy}),o.emit("selected",t))}))},S=function(e,n){if(n=n.originalOption,s(I.value,n,{valueBy:a.valueBy}))I.value=f(I.value,n,{min:a.min,valueBy:a.valueBy}),o.emit("removed",n);else{if(!t.multiple&&1===I.value.length){var l=I.value[0];I.value=f(I.value,I.value[0],{min:0,valueBy:a.valueBy}),o.emit("removed",l)}I.value=v(I.value,n,{max:a.max,valueBy:a.valueBy}),o.emit("selected",n)}},F=function(){if(!k()){var e=I.value.map((function(e){return a.valueBy(e)}));t.multiple?o.emit("update:modelValue",e):e.length?o.emit("update:modelValue",e[0]):o.emit("update:modelValue",t.emptyModelValue)}},O=function(){d.value.value="",d.value.dispatchEvent(new Event("input")),d.value.dispatchEvent(new Event("change"))},C=e.computed((function(){var e,n;return null!==(e=null!==(n=t.visibleOptions)&&void 0!==n?n:w.value)&&void 0!==e?e:a.options})),A=e.ref(0),T=e.computed((function(){var e,n=new Set(C.value.map((function(e){return a.valueBy(e)}))),o=a.options.map((function(e,o){var l={key:a.valueBy(e),label:a.labelBy(e),group:a.groupBy(e),originalIndex:o,originalOption:e};return l.selected=l.group?e.value.every((function(e){return x.value.has(e)})):x.value.has(a.valueBy(e)),l.disabled=l.group?a.disabledBy(e)||e.value.every((function(e){var t=p(a.options,e,{valueBy:a.valueBy});return a.disabledBy(t)})):a.disabledBy(e),l.visible=l.group?e.value.some((function(e){return n.has(e)})):n.has(a.valueBy(e)),l.hidden=!!t.hideSelected&&(l.group?e.value.every((function(e){return x.value.has(e)})):x.value.has(a.valueBy(e))),l})),r=l(o);try{for(r.s();!(e=r.n()).done;){var i=e.value;!1!==i.group&&(i.disabled&&function(){var e=new Set(i.originalOption.value);o.filter((function(t){return e.has(a.valueBy(t.originalOption))})).forEach((function(e){return e.disabled=!0}))}())}}catch(e){r.e(e)}finally{r.f()}return o})),D=function(t,n){var o=e.computed((function(){return t.value.reduce((function(e,t){var n;return Object.assign(e,((n={})[t.originalIndex]=t,n))}),{})})),a=function(e){var t=o.value[e];return void 0!==t&&!1!==l(t)&&(n.value=e,!0)},l=function(e){return!e.disabled&&!e.hidden&&e.visible},r=e.computed((function(){return t.value.some((function(e){return l(e)}))}));return e.watchEffect((function(){if(!1===r.value&&(n.value=null),t.value.length<=n.value)for(var e=0,o=t.value.reverse();e<o.length;e++){var i=o[e];if(a(i.originalIndex))break}if(null===n.value||!1===l(t.value[n.value]))for(var u=0,c=t.value;u<c.length&&(i=c[u],!a(i.originalIndex));u++);})),{pointerForward:function(){if(!1!==r.value&&null!==n.value)for(var e=n.value+1,o=0;e!==n.value&&o++<t.value.length&&(t.value.length<=e&&(e=0),!a(e));)++e},pointerBackward:function(){if(!1!==r.value&&null!==n.value)for(var e=n.value-1,o=0;e!==n.value&&o++<t.value.length&&(e<0&&(e=t.value.length-1),!a(e));)--e},pointerSet:a}}(T,A),E=D.pointerForward,L=D.pointerBackward,j=D.pointerSet,$="",U=/^[\w]$/,z=e.computed((function(){var e=n(a.options.keys());return e.slice(A.value).concat(e.slice(0,A.value))})),P=function(){var e,t=null===(e=i.value)||void 0===e?void 0:e.querySelector(".highlighted");if(t&&u.value){var n,o=getComputedStyle(t);for(n=0;t.offsetTop+parseFloat(o.height)+parseFloat(o.paddingTop)+parseFloat(o.paddingBottom)>u.value.$el.clientHeight+u.value.$el.scrollTop&&n++<T.value.length;)u.value.$el.scrollTop=u.value.$el.scrollTop+parseFloat(o.height)+parseFloat(o.paddingTop)+parseFloat(o.paddingBottom);for(n=0;t.offsetTop<u.value.$el.scrollTop&&n++<T.value.length;)u.value.$el.scrollTop=u.value.$el.scrollTop-parseFloat(o.height)-parseFloat(o.paddingTop)-parseFloat(o.paddingBottom)}};e.watch((function(){return[h.value,a.options,x.value]}),(function(t,n){!0!==(null==n?void 0:n[0])&&!1!==h.value&&0!==I.value.length&&(j(a.options.findIndex((function(e){return x.value.has(a.valueBy(e))}))),e.nextTick(P))}),{deep:!0,immediate:!0});var R=e.computed((function(){return{"data-is-focusing":h.value,"data-visible-length":T.value.filter((function(e){return e.visible&&!1===e.hidden})).length,"data-not-selected-length":a.options.length-T.value.filter((function(e){return e.selected})).length,"data-selected-length":T.value.filter((function(e){return e.selected})).length,"data-addable":T.value.filter((function(e){return e.selected})).length<a.max,"data-removable":T.value.filter((function(e){return e.selected})).length>a.min,"data-total-length":a.options.length,"data-multiple":t.multiple,"data-loading":t.loading,"data-disabled":t.disabled}}));e.provide("dataAttrs",R);var H=e.computed((function(){var e=T.value.filter((function(e){return e.selected})).filter((function(e){return!e.group}));return t.multiple?0===e.length?t.placeholder:1===e.length?"1 option selected":e.length+" options selected":0===e.length?t.placeholder:e[0].label+""})),_=e.ref();return e.watch((function(){return[t.openDirection,h.value]}),(function(){var e,n;_.value=null!==(e=null!==(n=t.openDirection)&&void 0!==n?n:function(){if(void 0===i.value)return;if(void 0===window)return;return window.innerHeight-i.value.getBoundingClientRect().bottom>=t.maxHeight?"bottom":"top"}())&&void 0!==e?e:"bottom"}),{immediate:!0}),{instance:r,isFocusing:h,wrapper:i,dropdown:u,input:c,focus:m,blur:y,toggle:function(){h.value?y():m()},searchingInputValue:b,handleInputForInput:function(e){o.emit("search:input",e)},handleChangeForInput:function(e){o.emit("search:change",e)},handleFocusForInput:function(e){m()},handleBlurForInput:function(e){y()},optionsWithInfo:T,addOrRemoveOption:function(e,n){t.disabled||(n.group&&t.multiple?N(e,n):S(e,n),F(),!0===t.closeOnSelect&&(h.value=!1),!0===t.clearOnSelect&&b.value&&O())},dataAttrs:R,innerPlaceholder:H,highlightedOriginalIndex:A,pointerForward:function(){E.apply(void 0,arguments),e.nextTick(P)},pointerBackward:function(){L.apply(void 0,arguments),e.nextTick(P)},pointerFirst:function(){var t,n=l(a.options.keys());try{for(n.s();!(t=n.n()).done;){var o=t.value;if(j(o))break}}catch(e){n.e(e)}finally{n.f()}e.nextTick(P)},pointerLast:function(){var t,o=l(n(a.options.keys()).reverse());try{for(o.s();!(t=o.n()).done;){var r=t.value;if(j(r))break}}catch(e){o.e(e)}finally{o.f()}e.nextTick(P)},typeAhead:function(e){if(!t.searchable){var n=!1;if(U.test(e.key)?($+=e.key.toLowerCase(),n=!0):"Space"===e.code&&($+=" "),n){var o,r=l(z.value);try{for(r.s();!(o=r.n()).done;){var i,u,c=o.value;if(!0===(null===(i=a.labelBy(a.options[c]))||void 0===i||null===(u=i.toLowerCase())||void 0===u?void 0:u.startsWith($))&&j(c))break}}catch(e){r.e(e)}finally{r.f()}clearTimeout(M),M=setTimeout((function(){$=""}),500)}}},pointerSet:j,direction:_}},components:{VInput:r,VTags:u,VDropdown:c},__VERSION__:"2.8.0"},m={ref:"header",class:"vue-select-header"},y={key:0,class:"vue-input"},b=e.createVNode("span",{class:"icon loading"},[e.createVNode("div"),e.createVNode("div"),e.createVNode("div")],-1),B={key:0,class:"vue-select-input-wrapper"},w=e.createVNode("span",{class:"icon loading"},[e.createVNode("div"),e.createVNode("div"),e.createVNode("div")],-1);return h.render=function(t,n,o,a,l,r){var i=e.resolveComponent("v-tags"),u=e.resolveComponent("v-input"),c=e.resolveComponent("v-dropdown");return e.openBlock(),e.createBlock("div",e.mergeProps({ref:"wrapper",class:["vue-select",["direction-".concat(t.direction)]],tabindex:t.isFocusing?-1:t.tabindex,onFocus:n[10]||(n[10]=function(){return t.focus&&t.focus.apply(t,arguments)}),onBlur:n[11]||(n[11]=function(e){return!t.searchable&&t.blur(e)})},Object.assign({},t.dataAttrs,t.$attrs),{onKeypress:n[12]||(n[12]=e.withKeys(e.withModifiers((function(){return null!==t.highlightedOriginalIndex&&t.addOrRemoveOption(t.$event,t.optionsWithInfo[t.highlightedOriginalIndex])}),["prevent","exact"]),["enter"])),onKeydown:[n[13]||(n[13]=e.withKeys(e.withModifiers((function(){return t.pointerForward&&t.pointerForward.apply(t,arguments)}),["prevent","exact"]),["down"])),n[14]||(n[14]=e.withKeys(e.withModifiers((function(){return t.pointerBackward&&t.pointerBackward.apply(t,arguments)}),["prevent","exact"]),["up"])),n[15]||(n[15]=e.withKeys(e.withModifiers((function(){return t.pointerFirst&&t.pointerFirst.apply(t,arguments)}),["prevent","exact"]),["home"])),n[16]||(n[16]=e.withKeys(e.withModifiers((function(){return t.pointerLast&&t.pointerLast.apply(t,arguments)}),["prevent","exact"]),["end"])),n[17]||(n[17]=function(){return t.typeAhead&&t.typeAhead.apply(t,arguments)})],id:"vs".concat(t.instance.uid,"-combobox"),role:t.searchable?"combobox":null,"aria-expanded":t.isFocusing,"aria-haspopup":"listbox","aria-owns":"vs".concat(t.instance.uid,"-listbox"),"aria-activedescendant":null===t.highlightedOriginalIndex?null:"vs".concat(t.instance.uid,"-option-").concat(t.highlightedOriginalIndex),"aria-busy":t.loading,"aria-disabled":t.disabled}),[e.createVNode("div",m,[t.multiple&&t.taggable&&0===t.modelValue.length||!1===t.searchable&&!1===t.taggable?(e.openBlock(),e.createBlock("div",y,[e.createVNode("input",{placeholder:t.innerPlaceholder,readonly:"",onClick:n[1]||(n[1]=function(){return t.focus&&t.focus.apply(t,arguments)})},null,8,["placeholder"])])):e.createCommentVNode("v-if",!0),t.multiple&&t.taggable?(e.openBlock(),e.createBlock(e.Fragment,{key:1},[e.createVNode(i,{modelValue:t.optionsWithInfo,"collapse-tags":t.collapseTags,tabindex:"-1",onClick:t.focus},{default:e.withCtx((function(n){var o=n.option;return[e.renderSlot(t.$slots,"tag",{option:o.originalOption,remove:function(){return t.addOrRemoveOption(t.$event,o)}},(function(){return[e.createVNode("span",null,e.toDisplayString(o.label),1),e.createVNode("img",{src:"data:image/svg+xml;base64,PHN2ZyBpZD0iZGVsZXRlIiBkYXRhLW5hbWU9ImRlbGV0ZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB2aWV3Qm94PSIwIDAgNTEyIDUxMiI+PHRpdGxlPmRlbGV0ZTwvdGl0bGU+PHBhdGggZD0iTTI1NiwyNEMzODMuOSwyNCw0ODgsMTI4LjEsNDg4LDI1NlMzODMuOSw0ODgsMjU2LDQ4OCwyNC4wNiwzODMuOSwyNC4wNiwyNTYsMTI4LjEsMjQsMjU2LDI0Wk0wLDI1NkMwLDM5Ny4xNiwxMTQuODQsNTEyLDI1Niw1MTJTNTEyLDM5Ny4xNiw1MTIsMjU2LDM5Ny4xNiwwLDI1NiwwLDAsMTE0Ljg0LDAsMjU2WiIgZmlsbD0iIzViNWI1ZiIvPjxwb2x5Z29uIHBvaW50cz0iMzgyIDE3Mi43MiAzMzkuMjkgMTMwLjAxIDI1NiAyMTMuMjkgMTcyLjcyIDEzMC4wMSAxMzAuMDEgMTcyLjcyIDIxMy4yOSAyNTYgMTMwLjAxIDMzOS4yOCAxNzIuNzIgMzgyIDI1NiAyOTguNzEgMzM5LjI5IDM4MS45OSAzODIgMzM5LjI4IDI5OC43MSAyNTYgMzgyIDE3Mi43MiIgZmlsbD0iIzViNWI1ZiIvPjwvc3ZnPg==",alt:"delete tag",class:"icon delete",onClick:e.withModifiers((function(){return t.addOrRemoveOption(t.$event,o)}),["prevent","stop"])},null,8,["onClick"])]}))]})),_:1},8,["modelValue","collapse-tags","onClick"]),e.renderSlot(t.$slots,"toggle",{isFocusing:t.isFocusing,toggle:t.toggle},(function(){return[e.createVNode("span",{class:["icon arrow-downward",{active:t.isFocusing}],onClick:n[2]||(n[2]=function(){return t.toggle&&t.toggle.apply(t,arguments)}),onMousedown:n[3]||(n[3]=e.withModifiers((function(){}),["prevent","stop"]))},null,34)]}))],64)):(e.openBlock(),e.createBlock(e.Fragment,{key:2},[t.searchable?(e.openBlock(),e.createBlock(u,{key:0,ref:"input",modelValue:t.searchingInputValue,"onUpdate:modelValue":n[4]||(n[4]=function(e){return t.searchingInputValue=e}),disabled:t.disabled,placeholder:t.isFocusing?t.searchPlaceholder:t.innerPlaceholder,onInput:t.handleInputForInput,onChange:t.handleChangeForInput,onFocus:t.handleFocusForInput,onBlur:t.handleBlurForInput,onEscape:t.blur,autofocus:t.autofocus||t.taggable&&t.searchable,tabindex:t.tabindex,comboboxUid:t.instance.uid},null,8,["modelValue","disabled","placeholder","onInput","onChange","onFocus","onBlur","onEscape","autofocus","tabindex","comboboxUid"])):e.createCommentVNode("v-if",!0),t.loading?e.renderSlot(t.$slots,"loading",{key:1},(function(){return[b]})):e.renderSlot(t.$slots,"toggle",{key:2,isFocusing:t.isFocusing,toggle:t.toggle},(function(){return[e.createVNode("span",{class:["icon arrow-downward",{active:t.isFocusing}],onClick:n[5]||(n[5]=function(){return t.toggle&&t.toggle.apply(t,arguments)}),onMousedown:n[6]||(n[6]=e.withModifiers((function(){}),["prevent","stop"]))},null,34)]}))],64))],512),t.multiple&&t.taggable&&t.searchable?(e.openBlock(),e.createBlock("div",B,[e.withDirectives(e.createVNode(u,{ref:"input",modelValue:t.searchingInputValue,"onUpdate:modelValue":n[7]||(n[7]=function(e){return t.searchingInputValue=e}),disabled:t.disabled,placeholder:t.isFocusing?t.searchPlaceholder:t.innerPlaceholder,onInput:t.handleInputForInput,onChange:t.handleChangeForInput,onFocus:t.handleFocusForInput,onBlur:t.handleBlurForInput,onEscape:t.blur,autofocus:t.autofocus||t.taggable&&t.searchable,tabindex:t.tabindex,comboboxUid:t.instance.uid},null,8,["modelValue","disabled","placeholder","onInput","onChange","onFocus","onBlur","onEscape","autofocus","tabindex","comboboxUid"]),[[e.vShow,t.isFocusing]]),t.loading?e.renderSlot(t.$slots,"loading",{key:0},(function(){return[w]})):e.createCommentVNode("v-if",!0)])):e.createCommentVNode("v-if",!0),e.createVNode(c,{ref:"dropdown",modelValue:t.optionsWithInfo,"onUpdate:modelValue":n[8]||(n[8]=function(e){return t.optionsWithInfo=e}),onClickItem:t.addOrRemoveOption,onMouseenter:n[9]||(n[9]=function(e,n){return t.pointerSet(n.originalIndex)}),comboboxUid:t.instance.uid,maxHeight:t.maxHeight,highlightedOriginalIndex:t.highlightedOriginalIndex},{default:e.withCtx((function(n){var o=n.option;return[e.renderSlot(t.$slots,"dropdown-item",{option:o.originalOption},(function(){return[e.createVNode("span",null,e.toDisplayString(o.label),1)]}))]})),_:1},8,["modelValue","onClickItem","comboboxUid","maxHeight","highlightedOriginalIndex"])],16,["tabindex","id","role","aria-expanded","aria-owns","aria-activedescendant","aria-busy","aria-disabled"])},h.__file="src/index.vue",h}(Vue);
