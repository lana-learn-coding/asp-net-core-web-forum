<template>
  <v-hover #default="{ hover }">
    <span :class="{icon: true, 'indigo--text': hover}" @click="$router.push({ name: 'Home' })">
      <span class="mr-1">Dr. Forum</span>
      <v-icon :color="hover ? 'indigo' : ''">masks</v-icon>
    </span>
  </v-hover>
</template>

<script>
export default {
  name: 'AppIcon',
};
</script>

<style scoped lang="scss">
.icon {
  font-size: 1.25rem;
  line-height: 1.5;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;

  &:hover {
    cursor: pointer;
  }
}
</style>
