function dashboardLoader(container, componentSelector, underscore) {
    this.container = container;
    this.componentSelector = componentSelector;
    this._ = underscore;

    dashboardLoader.prototype.InitializeComponent = function (selector) {
        selector.html('');
    }

    dashboardLoader.prototype.AddComponents = function (availableComponents, selectedComponents, componentHandler, noComponentSpecifiedHandler) {
        var that = this;
        var componentContainers = that.container.find(that.componentSelector);

        if (componentContainers.length > 0) {
            that._.each(componentContainers, function (component, idx) {
                var currentComponent = that._.chain(selectedComponents).find(function (x) { return x.index == idx; }).value();

                if ((!that._.isUndefined(currentComponent)) && (availableComponents[currentComponent.name]))
                    componentHandler(that, component, availableComponents[currentComponent.name]);
            });
        }
    };

    dashboardLoader.prototype.RenderComponent = function (selector, chartElem) {
        var that = this;

        that.container.find(selector).append(chartElem);
    };
}



