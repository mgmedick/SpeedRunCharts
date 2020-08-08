function dashboardLoader(container, componentSelector) {
    this.container = container;
    this.componentSelector = componentSelector;

    dashboardLoader.prototype.InitializeComponent = function (selector) {
        selector.html('');
    }

    dashboardLoader.prototype.AddComponents = function (availableComponents, selectedComponents, componentHandler, noComponentSpecifiedHandler) {
        var that = this;
        var componentContainers = that.container.find(that.componentSelector);

        _.each(componentContainers, function (componentContainer, idx) {
            var currentComponent = _.chain(selectedComponents).find(function (x) { return x.index == idx; }).value();

            if (currentComponent && availableComponents[currentComponent.name]) {
                componentHandler(componentContainer, availableComponents[currentComponent.name]);
            }
        });
    };

    dashboardLoader.prototype.RenderComponent = function (selector, chartElem) {
        var that = this;

        that.container.find(selector).append(chartElem);
    };
}



