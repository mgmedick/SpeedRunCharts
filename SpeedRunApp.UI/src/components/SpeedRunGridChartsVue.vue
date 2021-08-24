<template>
  <div id="divChartContainer">
      <fusioncharts :type="msline" :width="550" :height="350" :renderAt="divChartContainer" :dataformat="json" :dataSource="getGameSpeedRunsByMonthData"></fusioncharts>
  </div>
</template>
<script>
export default {
  name: 'app',
  props: {
    tabledata: Array
  },
  data() {
    return {
      "type": "msline",
      "renderAt": "chart-container",
      "width": "550",
      "height": "350",
      "dataFormat": "json",
      dataSource
    }
  }, 
  methods: {
    getGameSpeedRunsByMonthData() {
      var that = this;
      this.loading = true;

      axios.get('../SpeedRun/GetSpeedRunGridData', { params: { gameID: this.gameid, categoryTypeID: this.categorytypeid, categoryID: this.categoryid, levelID: this.levelid, variableValueIDs: this.variablevalues } })
          .then(res => {
              that.initGrid(res.data); 
              that.loading = false;                      
          })
          .catch(err => { console.error(err); return Promise.reject(err); });
    }    
  }
}
</script>

